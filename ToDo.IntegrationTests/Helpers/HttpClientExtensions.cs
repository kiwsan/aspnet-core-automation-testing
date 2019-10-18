using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using NUnit.Framework;

namespace ToDo.IntegrationTests.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton)
        {
            return client.SendAsync(form, submitButton, new Dictionary<string, string>());
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {

            Assert.That(form.QuerySelectorAll("[type=submit]") != null);

            var submitElement = form.QuerySelectorAll("[type=submit]");

            Assert.IsInstanceOf<IHtmlElement>(submitElement);

            var submitButton = submitElement as IHtmlElement;

            return client.SendAsync(form, submitButton, formValues);
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {
            foreach (var kvp in formValues)
            {
                Assert.IsInstanceOf<IHtmlInputElement>(form[kvp.Key]);

                var element = form[kvp.Key] as IHtmlInputElement;

                element.Value = kvp.Value;
            }

            var submit = form.GetSubmission(submitButton);
            var target = (Uri)submit.Target;
            if (submitButton.HasAttribute("formaction"))
            {
                var formaction = submitButton.GetAttribute("formaction");
                target = new Uri(formaction, UriKind.Relative);
            }
            var submision = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
            {
                Content = new StreamContent(submit.Body)
            };

            foreach (var header in submit.Headers)
            {
                submision.Headers.TryAddWithoutValidation(header.Key, header.Value);
                submision.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return client.SendAsync(submision);
        }
    }
}
