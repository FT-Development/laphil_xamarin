using System;


namespace LAPhil.Faq
{
    public interface IFaqDriver
    {
        IObservable<Faq[]> GetFaqs();
    }
}
