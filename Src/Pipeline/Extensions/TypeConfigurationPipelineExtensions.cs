using System;
using System.Configuration;
using Tamarack.Configuration;

namespace Tamarack.Pipeline.Extensions
{
    /// <summary>
    /// Class TypeConfigurationPipelineExtensions.
    /// </summary>
    public static class TypeConfigurationPipelineExtensions
    {
        /// <summary>
        /// Adds the configuration section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="section">The section.</param>
        /// <returns>Pipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddConfigurationSection<T, TOut>(this IPipeline<T, TOut> pipeline, string section)
        {
            return ForEachTypeIn(section, pipeline, (t, p) => t.Add(t));
        }

        /// <summary>
        /// Adds the configuration section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="section">The section.</param>
        /// <returns>ActionPipeline&lt;T&gt;.</returns>
        public static IPipeline<T> AddConfigurationSection<T>(this IPipeline<T> pipeline, string section)
        {
            return ForEachTypeIn(section, pipeline, (t, p) => t.Add(t));
        }

        public static IPublisher<T> AddConfigurationSection<T>(this IPublisher<T> pipeline, string section)
        {
            return ForEachTypeIn(section, pipeline, (t,p) => t.Add(t));
        }

        /// <summary>
        /// Fors the each type in.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentException">Configuration section '" + section + "' required</exception>
        private static TPipeline ForEachTypeIn<TPipeline>(string section, TPipeline pipeline, Func<TPipeline, Type, TPipeline> action)
        {
            var config = (TypeCollectionConfigurationSection)ConfigurationManager.GetSection(section);

            if (config == null)
                throw new ArgumentException("Configuration section '" + section + "' required");

            foreach (TypeConfigurationElement element in config.TypeCollection)
            {
                pipeline = action(pipeline, element.Type);
            }
            return pipeline;
        }
    }
}