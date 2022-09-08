using SerresApp.Interfaces;

namespace SerresApp.ViewModels
{
    public partial class ContentBaseViewModel : BaseViewModel
    {
        public IContentService _contentService { get; }

        public ContentBaseViewModel(IContentService contentService) : base(contentService)
        {
            _contentService = contentService;
        }
    }
}
