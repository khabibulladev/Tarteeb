//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Tickets.Exceptions
{
    public class InvalidTicketException : Xeption
    {
        public InvalidTicketException()
            : base(message: "Ticket is invalid.")
        { }
    }
}
