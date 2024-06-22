using System.Reflection;

namespace moviesAPI.FileTransform
{
    /// <summary>
    /// Helper class that reads font data from embedded resources.
    /// </summary>
    public static class FontHelper
    {
        public static byte[] Constantia
        {
            get { return LoadFontData("constantia.constan.ttf"); }
        }

        public static byte[] ConstantiaBold
        {
            get { return LoadFontData("constantia.constanb.ttf"); }
        }

        public static byte[] ConstantiaItalic
        {
            get { return LoadFontData("constantia.constani.ttf"); }
        }

        public static byte[] ConstantiaBoldItalic
        {
            get { return LoadFontData("constantia.constanz.ttf"); }
        }
        public static byte[] Arial
        {
            get { return LoadFontData("arial.arial.ttf"); }
        }

        public static byte[] ArialBold
        {
            get { return LoadFontData("arial.arialb.ttf"); }
        }

        public static byte[] ArialItalic
        {
            get { return LoadFontData("arial.ariali.ttf"); }
        }

        public static byte[] ArialBoldItalic
        {
            get { return LoadFontData("arial.arialz.ttf"); }
        }
        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        static byte[] LoadFontData(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts
            //var ourResources = assembly.GetManifestResourceNames();
            const string GLOBALPATH = "moviesAPI.Material.fonts.";

            using (Stream stream = assembly.GetManifestResourceStream(GLOBALPATH + name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + GLOBALPATH + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}
