using FluentValidation;
using System;
using System.Threading.Tasks;
using Xunit;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Domain.Models;

namespace Commentaries.Application.Test.RequestPartValidators;

public class HasObjectIdValidatorTests
{
    private readonly IValidator<IHasObjectId> _validator;

    public HasObjectIdValidatorTests(IValidator<IHasObjectId> validator)
    {
        _validator = validator;
    }

    [Fact]
    public async Task Should_BeValidInput_When_ObjectIdIsProvided()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectIdInput
            {
                ObjectId = Guid.NewGuid().ToString(),
            });

        Assert.Empty(validationResult.Errors);
    }

    [Fact]
    public async Task Should_BeInvalidInput_When_ObjectIdIsNotProvided()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectIdInput
            {
                ObjectId = string.Empty,
            });

        Assert.NotEmpty(validationResult.Errors);
        Assert.Contains(validationResult.Errors,
            e => e.ErrorCode == ExpectedErrorCodes.NotEmptyValidator);
    }

    [Fact]
    public async Task Should_BeInvalidInput_When_ObjectIdMaxLengthIsExceeded()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectIdInput
            {
                ObjectId = "#".PadRight(Comment.OBJECT_ID_MAX_LENGTH + 1),
            });

        Assert.NotEmpty(validationResult.Errors);
        Assert.Contains(validationResult.Errors,
            e => e.ErrorCode == ExpectedErrorCodes.MaximumLengthValidator);
    }

    private class HasObjectIdInput : IHasObjectId
    {
        public string ObjectId { get; set; } = string.Empty;
    }
}
