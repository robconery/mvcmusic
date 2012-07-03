using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace MvcMusicStore
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            // TODO: Configure your bundles here...
            // Please read http://getcassette.net/documentation/configuration

            // This default configuration treats each file as a separate 'bundle'.
            // In production the content will be minified, but the files are not combined.
            // So you probably want to tweak these defaults!
            bundles.Add<StylesheetBundle>("assets/css");
            bundles.Add<ScriptBundle>("assets/js");

            bundles.AddUrlWithAlias<ScriptBundle>("https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js", "jquery");
            bundles.AddUrlWithAlias<ScriptBundle>("https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js", "jquery-ui");
            bundles.AddUrlWithAlias<ScriptBundle>("http://ajax.aspnetcdn.com/ajax/knockout/knockout-2.1.0.js", "knockout");


            //<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
            //<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js"></script>
            //<script src="http://ajax.aspnetcdn.com/ajax/knockout/knockout-2.1.0.js"></script>


            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.
            
            // If you want a bundle per folder, try this:
            //   bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.

            // *** TOP TIP: Delete all ".min.js" files now ***
            // Cassette minifies scripts for you. So those files are never used.
        }
    }
}