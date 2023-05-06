using PdfSharp.Fonts;
using System.Reflection;

class MyFontResolver : IFontResolver
{
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Ignore case of font names.
        var name = familyName.ToLower().TrimEnd('#');

        // Deal with the fonts we know.
        switch (name)
        {
            case "constantia":
                if (isBold)
                {
                    if (isItalic)
                        return new FontResolverInfo("Constantia#bi");
                    return new FontResolverInfo("Constantia#b");
                }
                if (isItalic)
                    return new FontResolverInfo("Constantia#i");
                return new FontResolverInfo("Constantia#");
        }

        // We pass all other font requests to the default handler.
        // When running on a web server without sufficient permission, you can return a default font at this stage.
        return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
    }

    /// <summary>
    /// Return the font data for the fonts.
    /// </summary>
    public byte[] GetFont(string faceName)
    {
        switch (faceName)
        {
            case "Constantia#":
                return FontHelper.Constantia;

            case "Constantia#b":
                return FontHelper.ConstantiaBold;

            case "Constantia#i":
                return FontHelper.ConstantiaItalic;

            case "Constantia#bi":
                return FontHelper.ConstantiaBoldItalic;
        }

        return null;
    }


    internal static MyFontResolver OurGlobalFontResolver = null;

    /// <summary>
    /// Ensure the font resolver is only applied once (or an exception is thrown)
    /// </summary>
    internal static void Apply()
    {
        if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
        {
            if (OurGlobalFontResolver == null)
                OurGlobalFontResolver = new MyFontResolver();

            GlobalFontSettings.FontResolver = OurGlobalFontResolver;
        }
    }
}


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

    /// <summary>
    /// Returns the specified font from an embedded resource.
    /// </summary>
    static byte[] LoadFontData(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Test code to find the names of embedded fonts
        //var ourResources = assembly.GetManifestResourceNames();
        const string GLOBALPATH = "moviesAPI.fonts.";
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