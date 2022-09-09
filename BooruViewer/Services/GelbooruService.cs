using BooruApi.Models;
using BooruApi;

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

        public async Task OnInitializedAsync()
        {
            if (LastPostResponse is not null)
            {
                return;
            }

            await NewSearch("");
        }

        public async Task NewSearch(string tags)
        {
            var tagsClass = new List<Tag>();

            foreach (var item in tags.Split(' '))
            {
                tagsClass.Add(new Tag()
                {
                    Value = item,
                });
            }

            var helper = new GelbooruPostQueryHelper()
            {
                Tags = tagsClass,
                Limit = 20,
                Page = 0,
                //Sort = new Sort()
                //{
                //    Type = SortType.Random,
                //},
                Rating = new List<RatingGelbooru>()
                {
                    new RatingGelbooru()
                    {
                        PostRating = GelbooruPostRating.General,
                        SearchType = SearchType.Include
                    }
                },
            };

            await NewSearch(helper);
        }

        public async Task NewSearch(GelbooruPostQueryHelper GelbooruPostQueryHelper)
        {
            GelbooruPostQueryHelper.Page = 0;
            this.GelbooruPostQueryHelper = GelbooruPostQueryHelper;

            Posts = new List<Post>();

            await LoadPosts();
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
