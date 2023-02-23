using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Domain.Test.RequestPartValidators;

public class HasObjectTypeFullNameInputValidatorTests
{
    private readonly IValidator<IHasObjectTypeFullName> _validator;

    public HasObjectTypeFullNameInputValidatorTests(
        IValidator<IHasObjectTypeFullName> validator)
    {
        _validator = validator;
    }

    [Fact]
    public async Task Should_BeValidInput_When_ObjectTypeFullNameIsProvided()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectTypeFullNameInput
            {
                ObjectTypeFullName = "SomeNamespace.SomeClassName",
            });

        Assert.Empty(validationResult.Errors);
    }

    [Fact]
    public async Task Should_BeInvalidInput_When_ObjectTypeFullNameIsNotProvided()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectTypeFullNameInput
            {
                ObjectTypeFullName = string.Empty,
            });

        Assert.NotEmpty(validationResult.Errors);
        Assert.Contains(validationResult.Errors,
            e => e.ErrorCode == ExpectedErrorCodes.NotEmptyValidator);
    }

    [Fact]
    public async Task Should_BeInvalidInput_When_OnlyObjectTypeNameIsProvided()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectTypeFullNameInput
            {
                ObjectTypeFullName = "SomeClassName",
            });

        Assert.NotEmpty(validationResult.Errors);
        Assert.Contains(validationResult.Errors,
            e => e.ErrorCode == ExpectedErrorCodes.ObjectTypeFullNameValidator);
    }

    [Fact]
    public async Task Should_BeInvalidInput_When_ObjectTypeFullNameMaxLengthIsExceeded()
    {
        var validationResult = await _validator
            .ValidateAsync(new HasObjectTypeFullNameInput
            {
                ObjectTypeFullName = "#".PadRight(ObjectType.FULL_NAME_MAX_LENGTH + 1),
            });

        Assert.NotEmpty(validationResult.Errors);
        Assert.Contains(validationResult.Errors,
            e => e.ErrorCode == ExpectedErrorCodes.MaximumLengthValidator);
    }

    private class HasObjectTypeFullNameInput : IHasObjectTypeFullName
    {
        public string ObjectTypeFullName { get; set; } = string.Empty;
    }
}
