﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptionis;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidScoreId = Guid.Empty;
            var invalidScoreException = new InvalidScoreExpetion();

            invalidScoreException.AddData(
                key: nameof(invalidScoreId), 
                values: "Id is required.");

            var expectedValidationScoreException = new 
                ScoreValidationException(invalidScoreException);
            
            // when 
            ValueTask<Score> retrieveScoreByIdAsync = 
                this.scoreService.RetrieveScoreByIdAsync(invalidScoreId);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(retrieveScoreByIdAsync.AsTask);

            // then 
            actualScoreValidationException.Should().BeEquivalentTo(expectedValidationScoreException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedValidationScoreException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectScoreByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
