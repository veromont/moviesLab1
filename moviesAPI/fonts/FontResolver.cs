using moviesAPI.fonts;
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
            case "arial":
                if (isBold)
                {
                    if (isItalic)
                        return new FontResolverInfo("Arial#bi");
                    return new FontResolverInfo("Arial#b");
                }
                if (isItalic)
                    return new FontResolverInfo("Arial#i");
                return new FontResolverInfo("Arial#");
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


            case "Arial#":
                return FontHelper.Arial;

            case "Arial#b":
                return FontHelper.ArialBold;

            case "Arial#i":
                return FontHelper.ArialItalic;

            case "Arial#bi":
                return FontHelper.ArialBoldItalic;
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