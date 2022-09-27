using BooruApi;
using BooruApi.Models.Gelbooru;
using BooruApi.Models.Gelbooru.PostEndpoint;

namespace BooruViewer.Services
{
    public class GelbooruService
    {
        private GelbooruApi GelbooruApi = new GelbooruApi();
        public ApiResponsePost LastPostResponse { get; private set; }
        public List<Post> Posts { get; private set; }
        public bool IsLoading { get; private set; }

        public void ClearLoadedResults()
        {
            Posts = new List<Post>();            
        }

        public async Task<bool> LoadNextPage(GelbooruPostQueryHelper GelbooruPostQueryHelper) 
        {
            if (IsLoading)
            {
                return false;
            }

            if (!LastPostResponse.Attributes.IsNextPageAvailable())
            {
                return false;
            }

            GelbooruPostQueryHelper.Page++;

            await LoadPosts(GelbooruPostQueryHelper);

            return true;
        }        

        public async Task LoadPosts(GelbooruPostQueryHelper GelbooruPostQueryHelper)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;

            // Rating safe + taks posible broken in gelboru's api

            // Wtf android workaround,
            // it doesn't let you use network on the main thread
            await Task.Run(async() =>
            {
                var url = GelbooruPostQueryHelper.ToString();

                LastPostResponse = await GelbooruApi.GetPostsAsync(GelbooruPostQueryHelper);

                Posts.AddRange(LastPostResponse.Posts);

                IsLoading = false;
            });
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
