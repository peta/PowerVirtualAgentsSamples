// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PVATestFramework.Console.Models.Activities
{
    public class ActivityList
    {
        [JsonProperty("list_of_conversations")]
        public List<ActivityList> list_of_conversations { get; set; }

        [JsonProperty("activities")]
        public List<Activity> Activities { get; set; }

        public ActivityList()
        {
            Activities = new List<Activity>();
            list_of_conversations = new List<ActivityList>();
        }
    }
    
    public class Activity
    {
        
        public string ValueType { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Timestamp { get; set; }
        public From From { get; set; }
        public string ChannelId { get; set; }
        public Value Value { get; set; }
        public string TextFormat { get; set; }
        public string Text { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string ReplyToId { get; set; }
        public List<object> SuggestedActions { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; }
    }

    public static class ActivityExtension
    {
        

        public static bool IsMessageActivityWithText(this Activity activity)
        {
            return activity.Type == "message" && !string.IsNullOrWhiteSpace(activity.Text);
        }

        public static Microsoft.Bot.Connector.DirectLine.Activity ToBotFrameworkActivity(this Activity activity)
        {
            var converted = new Microsoft.Bot.Connector.DirectLine.Activity
            {
                Type = activity.Type,
                Text = activity.Text,
                Name = activity.Name,
                Value = activity.Value
            };

            try
            {
                if (activity.Attachments?.Any() ?? false)
                    converted.Attachments = activity.Attachments.Select(x => 
                        JsonConvert.DeserializeObject<Microsoft.Bot.Connector.DirectLine.Attachment>(JsonConvert.SerializeObject(x, Formatting.None))).ToList();
            }
            catch (Exception exc)
            {
                throw new Exception("Failed to convert custom activity attachment model to Bot Framework variant", exc);
            }
            
            return converted;
        }
    }

    public class Attachment
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    public class Action
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tooltip", NullValueHandling = NullValueHandling.Ignore)]
        public string Tooltip { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Body
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
        public List<Column>? Columns { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string? Size { get; set; }

        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public string? Weight { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string? Text { get; set; }

        [JsonProperty("wrap", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Wrap { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Item>? Items { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string? Title { get; set; }

        [JsonProperty("valueOn", NullValueHandling = NullValueHandling.Ignore)]
        public string? ValueOn { get; set; }

        [JsonProperty("valueOff", NullValueHandling = NullValueHandling.Ignore)]
        public string? ValueOff { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string? Label { get; set; }

        [JsonProperty("spacing", NullValueHandling = NullValueHandling.Ignore)]
        public string? Spacing { get; set; }

        [JsonProperty("fontType", NullValueHandling = NullValueHandling.Ignore)]
        public string? FontType { get; set; }

    }

    public class Column
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }
    }

    public class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("body")]
        public List<Body> Body { get; set; }

        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Action>? Actions { get; set; }

        [JsonProperty("$schema")]
        public string Schema { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("actionSubmitId", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionSubmitId { get; set; }
    }

    public class Item
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string? Url { get; set; }

        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Action>? Actions { get; set; }
    }

    public class From
    {
        public From(string id, int role)
        {
            Id = id;
            Role = role;
        }

        public string Id { get; set; }
        public int Role { get; set; }
    }

    public class Value
    {
        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; set; } = new Dictionary<string, object>();

        [JsonProperty("triggerUtterance")]
        public string TriggerUtterance
        {
            get; 
            set;
        }

        [JsonProperty("normalizedTriggerUtterance")]
        public string NormalizedTriggerUtterance { get; set; }
        
        [JsonProperty("intentCandidates")]
        public List<IntentCandidate> IntentCandidates { get; set; }

        public object ToObject123()
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this));
        }
    }

    public class IntentCandidate
    {
        [JsonProperty("intentId")]
        public string IntentId { get; set; }

        [JsonProperty("intentScore")]
        public IntentScore IntentScore { get; set; }
    }

    public class IntentScore
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("Type")]
        public int Type { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}
