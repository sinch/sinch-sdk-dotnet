using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Apps.Create;
using Sinch.Conversation.Apps.Credentials;
using Sinch.Conversation.Apps.Update;
using Sinch.Conversation.Common;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class AppsTests : ConversationTestBase
    {
        private object _app = new
        {
            channel_credentials = new[]
            {
                new
                {
                    callback_secret = "my_callback_secret",
                    credential_ordinal_number = 0,
                    channel = "WHATSAPP",
                    mms_credentials = new
                    {
                        account_id = "my_account_id",
                        api_key = "my_api_key",
                        basic_auth = new
                        {
                            password = "my_password",
                            username = "my_username"
                        }
                    },
                    kakaotalk_credentials = new
                    {
                        kakaotalk_plus_friend_id = "my_kakaotalk_id",
                        kakaotalk_sender_key = "my_kakaotalk_key"
                    },
                    static_bearer = new
                    {
                        claimed_identity = "my_claimed_identity",
                        token = "my_static_bearer_token"
                    },
                    static_token = new
                    {
                        token = "my_static_token"
                    },
                    telegram_credentials = new
                    {
                        token = "my_telegram_bot_token"
                    },
                    line_credentials = new
                    {
                        token = "my_line_token",
                        secret = "my_line_secret"
                    },
                    wechat_credentials = new
                    {
                        app_id = "my_wechat_app_id",
                        app_secret = "my_wechat_app_secret",
                        token = "my_wechat_token",
                        aes_key = "my_wechat_aes_key"
                    }
                }
            },
            conversation_metadata_report_view = "NONE",
            display_name = "Sinch Conversation API Demo App 001",
            id = "{APP_ID}",
            rate_limits = new
            {
                inbound = 100,
                outbound = 200,
                webhooks = 50
            },
            retention_policy = new
            {
                retention_type = "MESSAGE_EXPIRE_POLICY",
                ttl_days = 180
            },
            dispatch_retention_policy = new
            {
                retention_type = "MESSAGE_EXPIRE_POLICY",
                ttl_days = 0
            },
            processing_mode = "CONVERSATION",
            smart_conversation = new
            {
                enabled = false
            },
            queue_stats = new
            {
                outbound_size = 10,
                outbound_limit = 20
            },
            message_retry_settings = new
            {
                retry_duration = 360,
            },
            callback_settings = new
            {
                secret_for_overridden_callback_urls = "secret",
            },
            delivery_report_based_fallback = new
            {
                enabled = true,
                delivery_report_waiting_time = 220,
            }
        };

        private object _createApp = new
        {
            display_name = "display_name",
            channel_credentials = new[]
            {
                new
                {
                    channel = "INSTAGRAM",
                    credential_ordinal_number = 0,
                    static_token = new
                    {
                        token = "token"
                    },
                    telegram_credentials = new
                    {
                        token = "tok"
                    },
                    wechat_credentials = new
                    {
                        token = "troc",
                        aes_key = "krok",
                        app_id = "block",
                        app_secret = "mrok"
                    },
                    line_credentials = new
                    {
                        secret = "sec",
                        token = "torc",
                        is_default = false,
                    },
                    callback_secret = "sec",
                    kakaotalk_credentials = new
                    {
                        kakaotalk_sender_key = "ole",
                        kakaotalk_plus_friend_id = "boke"
                    },
                    kakaotalkchat_credentials = new
                    {
                        api_key = "api_key",
                        kakaotalk_plus_friend_id = "friend_id"
                    },
                    mms_credentials = new
                    {
                        account_id = "acc_id",
                        api_key = "akey",
                        basic_auth = new
                        {
                            password = "123",
                            username = "456"
                        }
                    },
                    static_bearer = new
                    {
                        token = "a",
                        claimed_identity = "b"
                    }
                }
            },
            processing_mode = "CONVERSATION",
            retention_policy = new
            {
                retention_type = "MESSAGE_EXPIRE_POLICY",
                ttl_days = 180
            },
            smart_conversation = new
            {
                enabled = true
            },
            dispatch_retention_policy = new
            {
                retention_type = "MESSAGE_EXPIRE_POLICY",
                ttl_days = 20
            },
            conversation_metadata_report_view = "FULL",
            message_retry_settings = new
            {
                retry_duration = 360,
            },
            callback_settings = new
            {
                secret_for_overridden_callback_urls = "secret",
            },
            delivery_report_based_fallback = new
            {
                enabled = true,
                delivery_report_waiting_time = 220,
            }
        };

        private CreateAppRequest _createRequest = new CreateAppRequest
        {
            DisplayName = "display_name",
            ChannelCredentials = new List<ConversationChannelCredentials>()
            {
                new ConversationChannelCredentials()
                {
                    Channel = ConversationChannel.Instagram,
                    StaticToken = new StaticTokenCredentials
                    {
                        Token = "token"
                    },
                    TelegramCredentials = new TelegramCredentials()
                    {
                        Token = "tok"
                    },
                    WechatCredentials = new WeChatCredentials()
                    {
                        Token = "troc",
                        AesKey = "krok",
                        AppId = "block",
                        AppSecret = "mrok"
                    },
                    LineCredentials = new LineCredentials()
                    {
                        Secret = "sec",
                        Token = "torc"
                    },
                    CallbackSecret = "sec",
                    KakaoTalkCredentials = new KakaoTalkCredentials()
                    {
                        KakaoTalkSenderKey = "ole",
                        KakaoTalkPlusFriendId = "boke",
                    },
                    KakaoTalkChatCredentials = new KakaoTalkChatCredentials()
                    {
                        ApiKey = "api_key",
                        KakaoTalkPlusFriendId = "friend_id",
                    },
                    MmsCredentials = new MmsCredentials()
                    {
                        AccountId = "acc_id",
                        ApiKey = "akey",
                        BasicAuth = new BasicAuthCredential()
                        {
                            Password = "123",
                            Username = "456",
                        }
                    },
                    StaticBearer = new StaticBearerCredentials()
                    {
                        Token = "a",
                        ClaimedIdentity = "b"
                    }
                }
            },
            ProcessingMode = ProcessingMode.Conversation,
            RetentionPolicy = new RetentionPolicy()
            {
                RetentionType = RetentionType.MessageExpirePolicy,
                TtlDays = 180,
            },
            SmartConversation = new SmartConversation(true),
            DispatchRetentionPolicy = new DispatchRetentionPolicy()
            {
                RetentionType = DispatchRetentionPolicyType.MessageExpirePolicy,
                TtlDays = 20,
            },
            ConversationMetadataReportView = ConversationMetadataReportView.Full,
            MessageRetrySettings = new MessageRetrySettings()
            {
                RetryDuration = 360,
            },
            CallbackSettings = new CallbackSettings()
            {
                SecretForOverriddenCallbackUrls = "secret",
            },
            DeliveryReportBasedFallback = new DeliveryReportBasedFallback()
            {
                Enabled = true,
                DeliveryReportWaitingTime = 220
            }
        };

        [Fact]
        public async Task Create()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps")
                .WithJson(JsonConvert.SerializeObject(_createApp))
                .Respond(HttpStatusCode.OK, JsonContent.Create(_app));


            var response = await Conversation.Apps.Create(_createRequest);

            response.DisplayName.Should().Be("Sinch Conversation API Demo App 001");
            response.QueueStats.Should().BeEquivalentTo(new QueueStats()
            {
                OutboundLimit = 20,
                OutboundSize = 10,
            });
            response.RateLimits.Should().BeEquivalentTo(new RateLimits()
            {
                Webhooks = 50,
                Inbound = 100,
                Outbound = 200
            });
            response.Id.Should().Be("{APP_ID}");
            response.RetentionPolicy.Should().BeEquivalentTo(new RetentionPolicy()
            {
                RetentionType = RetentionType.MessageExpirePolicy,
                TtlDays = 180
            });
            response.DispatchRetentionPolicy.Should().BeEquivalentTo(new DispatchRetentionPolicy()
            {
                RetentionType = DispatchRetentionPolicyType.MessageExpirePolicy,
                TtlDays = 0,
            });
            response.ConversationMetadataReportView.Should().Be(ConversationMetadataReportView.None);
            response.ProcessingMode.Should().Be(ProcessingMode.Conversation);
            response.SmartConversation!.Enabled.Should().BeFalse();
            response.ChannelCredentials![0].Should().BeEquivalentTo(new ConversationChannelCredentials()
            {
                CallbackSecret = "my_callback_secret",
                Channel = ConversationChannel.WhatsApp,
                MmsCredentials = new MmsCredentials()
                {
                    AccountId = "my_account_id",
                    ApiKey = "my_api_key",
                    BasicAuth = new BasicAuthCredential()
                    {
                        Password = "my_password",
                        Username = "my_username",
                    },
                },
                KakaoTalkCredentials = new KakaoTalkCredentials()
                {
                    KakaoTalkSenderKey = "my_kakaotalk_key",
                    KakaoTalkPlusFriendId = "my_kakaotalk_id"
                },
                LineCredentials = new LineCredentials()
                {
                    Secret = "my_line_secret",
                    Token = "my_line_token"
                },
                StaticBearer = new StaticBearerCredentials()
                {
                    Token = "my_static_bearer_token",
                    ClaimedIdentity = "my_claimed_identity",
                },
                StaticToken = new StaticTokenCredentials()
                {
                    Token = "my_static_token"
                },
                TelegramCredentials = new TelegramCredentials()
                {
                    Token = "my_telegram_bot_token"
                },
                WechatCredentials = new WeChatCredentials()
                {
                    AppId = "my_wechat_app_id",
                    Token = "my_wechat_token",
                    AppSecret = "my_wechat_app_secret",
                    AesKey = "my_wechat_aes_key"
                }
            });
        }

        [Fact]
        public async Task List()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    apps = new[]
                    {
                        _app, _app
                    }
                }));

            var response = await Conversation.Apps.List();

            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/123")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_app));

            var response = await Conversation.Apps.Get("123");

            response.Should().BeEquivalentTo(new App
            {
                ChannelCredentials = new List<ConversationChannelCredentials>
                {
                    new ConversationChannelCredentials
                    {
                        CallbackSecret = "my_callback_secret",
                        CredentialOrdinalNumber = 0,
                        Channel = ConversationChannel.WhatsApp,
                        MmsCredentials = new MmsCredentials
                        {
                            AccountId = "my_account_id",
                            ApiKey = "my_api_key",
                            BasicAuth = new BasicAuthCredential
                            {
                                Password = "my_password",
                                Username = "my_username"
                            }
                        },
                        KakaoTalkCredentials = new KakaoTalkCredentials
                        {
                            KakaoTalkPlusFriendId = "my_kakaotalk_id",
                            KakaoTalkSenderKey = "my_kakaotalk_key"
                        },
                        StaticBearer = new StaticBearerCredentials()
                        {
                            ClaimedIdentity = "my_claimed_identity",
                            Token = "my_static_bearer_token"
                        },
                        StaticToken = new StaticTokenCredentials()
                        {
                            Token = "my_static_token"
                        },
                        TelegramCredentials = new TelegramCredentials
                        {
                            Token = "my_telegram_bot_token"
                        },
                        LineCredentials = new LineCredentials
                        {
                            Token = "my_line_token",
                            Secret = "my_line_secret"
                        },
                        WechatCredentials = new WeChatCredentials
                        {
                            AppId = "my_wechat_app_id",
                            AppSecret = "my_wechat_app_secret",
                            Token = "my_wechat_token",
                            AesKey = "my_wechat_aes_key"
                        }
                    }
                },
                ConversationMetadataReportView = ConversationMetadataReportView.None,
                DisplayName = "Sinch Conversation API Demo App 001",
                Id = "{APP_ID}",
                RateLimits = new RateLimits
                {
                    Inbound = 100,
                    Outbound = 200,
                    Webhooks = 50
                },
                RetentionPolicy = new RetentionPolicy
                {
                    RetentionType = RetentionType.MessageExpirePolicy,
                    TtlDays = 180
                },
                DispatchRetentionPolicy = new DispatchRetentionPolicy
                {
                    RetentionType = DispatchRetentionPolicyType.MessageExpirePolicy,
                    TtlDays = 0
                },
                ProcessingMode = ProcessingMode.Conversation,
                SmartConversation = new SmartConversation(false),
                QueueStats = new QueueStats
                {
                    OutboundSize = 10,
                    OutboundLimit = 20
                },
                MessageRetrySettings = new MessageRetrySettings
                {
                    RetryDuration = 360
                },
                CallbackSettings = new CallbackSettings
                {
                    SecretForOverriddenCallbackUrls = "secret"
                },
                DeliveryReportBasedFallback = new DeliveryReportBasedFallback
                {
                    Enabled = true,
                    DeliveryReportWaitingTime = 220
                }
            });
        }

        [Fact]
        public async Task Delete()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/123")
                .Respond(HttpStatusCode.OK);

            Func<Task> response = () => Conversation.Apps.Delete("123");

            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Update()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/123")
                .WithJson(JsonConvert.SerializeObject(new
                {
                    display_name = "abc"
                }))
                .WithQueryString("update_mask.paths", "a")
                .WithQueryString("update_mask.paths", "b")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_app));

            var request = new UpdateAppRequest()
            {
                DisplayName = "abc",
                UpdateMaskPaths = new List<string>()
                {
                    "a", "b"
                },
            };

            var response = await Conversation.Apps.Update("123", request);

            response.Should().NotBeNull();
        }

        [Fact]
        public void DeserializeConversationChannelCredentialsLineThailandEnterprise()
        {
            var json = Helpers.LoadResources(
                "Conversation/Apps/LineThailandEnterpriseCredentials.json");

            var result = DeserializeAsConversationClient<ConversationChannelCredentials>(json);

            result.Should().BeEquivalentTo(new ConversationChannelCredentials
            {
                Channel = ConversationChannel.Line,
                CallbackSecret = "callback secret",
                CredentialOrdinalNumber = 1,
                LineEnterpriseCredentials = new LineEnterpriseCredentials(new LineThailand()
                {
                    Token = "line enterprise credentials thailand token value",
                    Secret = "line enterprise credentials thailand secret value"
                })
                {
                    IsDefault = true
                }
            });
        }

        [Fact]
        public void DeserializeConversationChannelCredentialsLineJapanEnterprise()
        {
            var json = Helpers.LoadResources(
                "Conversation/Apps/LineJapanEnterpriseCredentials.json");

            var result = DeserializeAsConversationClient<ConversationChannelCredentials>(json);

            result.Should().BeEquivalentTo(new ConversationChannelCredentials
            {
                Channel = ConversationChannel.Line,
                CallbackSecret = "callback secret",
                CredentialOrdinalNumber = 1,
                ChannelKnownId = "channel id",
                State = new ChannelIntegrationState
                {
                    Status = ChannelIntegrationStatus.Pending,
                    Description = "description value"
                },
                LineEnterpriseCredentials = new LineEnterpriseCredentials(new LineJapan
                {
                    Token = "line enterprise credentials japan token value",
                    Secret = "line enterprise credentials japan secret value"
                })
                {
                    IsDefault = true
                }
            });
        }
    }
}
