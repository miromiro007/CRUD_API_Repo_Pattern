File: CRUD_API.Tests\ItemsControllerTests.cs
````````csharp
        [Fact]
        public async Task CreateItem_WithNullData_ReturnsBadRequest()
        {
            // 🟦 ARRANGE
            Item nullItem = null;

            // 🟩 ACT
            var result = await _controller.CreateItem(nullItem);

            // 🟧 ASSERT
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badResult.Value);
        }
    }
}