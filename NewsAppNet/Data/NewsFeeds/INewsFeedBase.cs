﻿using NewsAppNet.Models.DataModels;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public interface INewsFeedBase
    {
        SyndicationFeed ReadFeed();
        List<NewsItem> GetNewsItems();
        NewsItemBuilder BuildItem(SyndicationItem item);
        NewsItem GetItem(SyndicationItem item);
    }
}
