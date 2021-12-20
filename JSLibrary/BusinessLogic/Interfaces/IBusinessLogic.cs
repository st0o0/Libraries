using System;

namespace JSLibrary.BusinessLogic.Interfaces
{
    public interface IBusinessLogic : IDisposable
    {
        DateTime GetTime();
    }
}