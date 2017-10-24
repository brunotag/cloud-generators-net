﻿using System;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CloudModelGenerator
{
    class Program
    {
        static IConfigurationRoot Configuration { get; set; }

        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "content-types-generator",
                Description = "Generates Kentico Cloud Content Types as CSharp classes.",
            };

            var projectIdOption = app.Option("-p|--projectid", "Kentico Cloud Project ID.", CommandOptionType.SingleValue);
            var namespaceOption = app.Option("-n|--namespace", "Namespace name of the generated classes.", CommandOptionType.SingleValue);
            var outputDirOption = app.Option("-o|--outputdir", "Output directory for the generated files.", CommandOptionType.SingleValue);
            var fileNameSuffixOption = app.Option("-sf|--filenamesuffix", "Optionally add a suffix to generated filenames (e.g., News.cs becomes News.Generated.cs).", CommandOptionType.SingleValue);
            var generatePartials = app.Option("-gp|--generatepartials", "Generate partial classes for customisation (if this option is set filename suffix will default to Generated).", CommandOptionType.NoValue);
            var includeTypeProvider = app.Option("-t|--withtypeprovider", "Indicates whether the CustomTypeProvider class should be generated.", CommandOptionType.NoValue);
            var structuredModel = app.Option("-s|--structuredmodel", "Indicates whether the classes should be generated with types that represent structured data model.", CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appSettings.json", true)
                    .Add(new CommandLineOptionsProvider(app.Options));

                Configuration = builder.Build();

                //TODO: this will be retrieved from Configuration (using DI or something)
                var options = new CodeGeneratorOptions
                {
                    ProjectId = projectIdOption.Value() ?? (string.IsNullOrEmpty(Configuration["projectId"]) ? null : Configuration["projectId"]),
                    OutputDir = outputDirOption.Value() ?? (string.IsNullOrEmpty(Configuration["outputdir"]) ? "." : Configuration["outputdir"]),
                    Namespace = namespaceOption.Value() ?? (string.IsNullOrEmpty(Configuration["namespace"]) ? null : Configuration["namespace"]),
                    FileNameSuffix = fileNameSuffixOption.Value() ?? (string.IsNullOrEmpty(Configuration["filenameSuffix"]) ? null : Configuration["filenameSuffix"]),
                    GeneratePartials = generatePartials.HasValue() || Configuration.GetValue("generatePartials", false)
                };

                //TODO: this should be part of CodeGeneratorOptions as well
                var passedSetIncludeTypeProvider = includeTypeProvider.HasValue() || Configuration.GetValue("withTypeProvider", true);
                var passedSetStructuredModel = structuredModel.HasValue() || Configuration.GetValue("structuredModel", false);

                // No projectId was passed as an arg or set in the appSettings.config
                if (options.ProjectId == null)
                {
                    app.Error.WriteLine("Provide a Project ID!");
                    app.ShowHelp();

                    return 1;
                }
                
                var codeGenerator = new CodeGenerator(Options.Create(options));

                codeGenerator.GenerateContentTypeModels(passedSetStructuredModel);

                if (passedSetIncludeTypeProvider)
                {
                    codeGenerator.GenerateTypeProvider();
                }

                return 0;
            });

            app.HelpOption("-? | -h | --help");

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException e)
            {
                Console.WriteLine("Invalid arguments!");
                Console.WriteLine(e.Message);
                app.ShowHelp();
                return 1;
            }
        }
    }
}
