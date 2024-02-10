namespace IdentityDev.Services
{
    using System.Text;
    using System.Xml.Linq;
    using Boilerplate;
    using Boilerplate.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using IdentityDev.Constants;

    public class BrowserConfigService : IBrowserConfigService
    {
        private readonly IUrlHelper urlHelper;

        public BrowserConfigService(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        public string GetBrowserConfigXml()
        {
            // The URL to the 70x70 small tile image.
            string square70x70logoUrl = this.urlHelper.Content("~/img/icons/mstile-70x70.png");
            // The URL to the 150x150 medium tile image.
            string square150x150logoUrl = this.urlHelper.Content("~/img/icons/mstile-150x150.png");
            // The URL to the 310x310 large tile image.
            string square310x310logoUrl = this.urlHelper.Content("~/img/icons/mstile-310x310.png");
            // The URL to the 310x150 wide tile image.
            string wide310x150logoUrl = this.urlHelper.Content("~/img/icons/mstile-310x150.png");
            // The colour of the tile. This colour only shows if part of your images above are transparent.
            string tileColour = "#1E1E1E";
            // $Start-Feed$
            // Update the tile every 1440 minutes. Defines the frequency, in minutes, between poll requests. Must be
            // one of the following values: 30 (1/2 Hour), 60 (1 Hour), 360 (6 Hours), 720 (12 Hours), 1440 (24 Hours).
            int frequency = 1440;
            // Control notification cycling. Must be one of the following values:
            // 0: (default if there's only one notification) No tiles show notifications.
            // 1: (default if there are multiple notifications) Notifications cycle for all tile sizes.
            // 2: Notifications cycle for medium tiles only.
            // 3: Notifications cycle for wide tiles only.
            // 4: Notifications cycle for large tiles only.
            // 5: Notifications cycle for medium and wide tiles.
            // 6: Notifications cycle for medium and large tiles.
            // 7: Notifications cycle for large and wide tiles.
            int cycle = 1;
            // $End-Feed$

            // $Start-Feed-On$
            XDocument document = new XDocument(
                new XElement("browserconfig",
                    new XElement("msapplication",
                        new XElement("tile",
                            new XElement("square70x70logo",
                                new XAttribute("src", square70x70logoUrl)),
                            new XElement("square150x150logo",
                                new XAttribute("src", square150x150logoUrl)),
                            new XElement("square310x310logo",
                                new XAttribute("src", square310x310logoUrl)),
                            new XElement("wide310x150logo",
                                new XAttribute("src", wide310x150logoUrl)),
                            new XElement("TileColor", tileColour)),
                        new XElement("notification",
                            //new XElement("polling-uri",
                            //    new XAttribute("src", GetTileUrl(1))),
                            //new XElement("polling-uri2",
                            //    new XAttribute("src", GetTileUrl(2))),
                            //new XElement("polling-uri3",
                            //    new XAttribute("src", GetTileUrl(3))),
                            //new XElement("polling-uri4",
                            //    new XAttribute("src", GetTileUrl(4))),
                            //new XElement("polling-uri5",
                            //    new XAttribute("src", GetTileUrl(5))),
                            new XElement("frequency", frequency),
                            new XElement("cycle", cycle)))));
            // $End-Feed-On$
            // $Start-Feed-Off$
            // XDocument document = new XDocument(
            //     new XElement("browserconfig",
            //         new XElement("msapplication",
            //             new XElement("tile",
            //                 new XElement("square70x70logo",
            //                     new XAttribute("src", square70x70logoUrl)),
            //                 new XElement("square150x150logo",
            //                     new XAttribute("src", square150x150logoUrl)),
            //                 new XElement("square310x310logo",
            //                     new XAttribute("src", square310x310logoUrl)),
            //                 new XElement("wide310x150logo",
            //                     new XAttribute("src", wide310x150logoUrl)),
            //                 new XElement("TileColor", tileColour)))));
            // $End-Feed-Off$

            return document.ToString(Encoding.UTF8);
        }
    }
}
