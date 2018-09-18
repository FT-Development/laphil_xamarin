using System;
using System.Reactive.Linq;
using LAPhil.Application;
using LAPhil.Logging;


namespace LAPhil.Faq
{
    public class MockFaqDriver: IFaqDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MockFaqDriver>();

        public MockFaqDriver()
        {
            Log.Info("Initialized MockFaqDriver");
        }

        public IObservable<Faq[]> GetFaqs()
        {
            var faqs = new Faq[]{
                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/2/",
                    Question = "Whens should I arrive?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/1/",
                            Label = "Planning Your Visit"
                        }
                    }
                }, 

                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/1/",
                    Question = "How do I buy a gift card?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/1/",
                            Label = "Tickets"
                        }
                    }
                },

                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/3/",
                    Question = "What should I wear?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/3/",
                            Label = "About the Performance"
                        }
                    }
                },

                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/2/",
                    Question = "Can I change how my tickets are delivered?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/1/",
                            Label = "Tickets"
                        }
                    }
                },

                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/2/",
                    Question = "Are there any customs I need to know?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/1/",
                            Label = "About the Performance"
                        }
                    }
                },

                new Faq{
                    Url = "http://laphil-dev.herokuapp.com/api/faqs/2/",
                    Question = "May I bring children under 6?",
                    Answer = "<p>Lorem ipsum dolor sit amet.</p>",
                    Headline = new FaqHeadline{
                        Url = "http://laphil-dev.herokuapp.com/api/faq-headlines/1/",
                        Label = "FAQ Headline 1",
                        Category = new FaqCategory{
                            Url = "http://laphil-dev.herokuapp.com/api/faq-categories/1/",
                            Label = "Planning Your Visit"
                        }
                    }
                }
            };

            return Observable.Return(faqs);
        }
    }
}