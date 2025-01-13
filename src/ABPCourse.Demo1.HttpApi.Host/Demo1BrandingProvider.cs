using Microsoft.Extensions.Localization;
using ABPCourse.Demo1.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ABPCourse.Demo1;

[Dependency(ReplaceServices = true)]
public class Demo1BrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<Demo1Resource> _localizer;

    public Demo1BrandingProvider(IStringLocalizer<Demo1Resource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
