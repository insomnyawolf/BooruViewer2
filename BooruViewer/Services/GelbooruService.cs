using BooruApi;
using BooruApi.Models.Gelbooru;
using BooruApi.Models.Gelbooru.PostEndpoint;

namespace BooruViewer.Services
{
    public class GelbooruService
    {
        private GelbooruApi GelbooruApi = new GelbooruApi();
        public GelbooruPostQueryHelper GelbooruPostQueryHelper { get; set; }
        public ApiResponsePost LastPostResponse { get; private set; }
        public List<Post> Posts { get; private set; }
        public int CurrentPage { get; private set; }
        public bool IsLoadingNextPage { get; private set; }
        public bool IsLoadingNewSearch { get; private set; }

        public async Task OnInitializedAsync()
        {
            if (LastPostResponse is not null)
            {
                return;
            }

            // This is a workaround for search without tags
            await InitializeSearch("lappland_(arknights)");
        }

        public async Task InitializeSearch(string tags)
        {
            var tagsClass = new List<Tag>();

            foreach (var item in tags.Split(' '))
            {
                tagsClass.Add(new Tag()
                {
                    Value = item,
                });
            }

            GelbooruPostQueryHelper = new GelbooruPostQueryHelper()
            {
                Tags = tagsClass,
                Limit = 20,
                Page = 0,
                //Sort = new Sort()
                //{
                //    Type = SortType.Random,
                //},
                Rating = new RatingGelbooru()
                {
                    PostRating = GelbooruPostRating.General,
                },
            };

            await NewSearch();
        }

        public async Task NewSearch(GelbooruPostQueryHelper GelbooruPostQueryHelper)
        {
            this.GelbooruPostQueryHelper = GelbooruPostQueryHelper;

            await NewSearch();
        }

        public async Task NewSearch()
        {
            GelbooruPostQueryHelper.Page = 0;

            Posts = new List<Post>();

            IsLoadingNewSearch = true;

            await LoadPosts();

            IsLoadingNewSearch = false;
        }

        public async Task<bool> LoadNextPage() 
        {
            if (IsLoadingNextPage)
            {
                return false;
            }

            if (!IsNextPageAvailable())
            {
                return false;
            }

            GelbooruPostQueryHelper.Page++;

            await LoadPosts();

            return true;
        }        

        private async Task LoadPosts()
        {
            if (IsLoadingNextPage)
            {
                return;
            }

            IsLoadingNextPage = true;

            // Rating safe + taks posible broken in gelboru's api

            // Wtf android workaround,
            // it doesn't let you use network on the main thread
            await Task.Run(async() =>
            {
                var url = GelbooruPostQueryHelper.ToString();

                LastPostResponse = await GelbooruApi.GetPostsAsync(GelbooruPostQueryHelper);

                Posts.AddRange(LastPostResponse.Posts);

                IsLoadingNextPage = false;
            });
        }

        //private bool PreviousPageAvailable()
        //{
        //    return LastPostResponse.Attributes.Offset == 0;
        //}

        public bool IsNextPageAvailable()
        {
            var atributes = LastPostResponse.Attributes;
            return atributes.Offset + atributes.Limit < atributes.Count;
        }

        public async Task<List<AutoCompleteResponse>> GetTagsAutocomplete(string partialInput)
        {
            // Wtf android workaround,
            // it doesn't let you use network on the main thread
            List<AutoCompleteResponse> autocomplete = null;

            await Task.Run(async () =>
            {
                autocomplete = await GelbooruApi.GetAutoCompleteAsync(partialInput);                
            });

            return autocomplete;
        }
    }
}
