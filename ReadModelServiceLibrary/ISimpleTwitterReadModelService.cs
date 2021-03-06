﻿using System.Collections.Generic;
using System.ServiceModel;
using ReadModel;

namespace ReadModelServiceLibrary
{
    [ServiceContract]
    public interface ISimpleTwitterReadModelService
    {
        [OperationContract]
        IEnumerable<TweetIndexItem> GetTweets();

        [OperationContract]
        IEnumerable<ChannelIndexItem> GetChannels();

        [OperationContract]
        IEnumerable<UserIndexItem> GetUsers();
    }
}
