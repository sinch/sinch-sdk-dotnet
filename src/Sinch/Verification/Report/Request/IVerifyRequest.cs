﻿namespace Sinch.Verification.Report
{
    public abstract class IVerifyRequest
    {
        /// <summary>
        ///     The type of verification.
        /// </summary>
        public abstract string Method { get; }
    }
}
