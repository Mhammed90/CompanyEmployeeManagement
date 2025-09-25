using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyEmployees.Presentation.ModelBinders;

// custom model binder for IEnumerable
public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    { 
        // check if  is enumerable
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }


        // get extract the value from the request and check if is null or empty
        var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
        if (string.IsNullOrEmpty(valueProvider))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

         // get the type of IEnumrable 
        var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        // Prepare the  converter to the sent type
        var converter = TypeDescriptor.GetConverter(genericType);
        // split and trim the white spaces and convert to array
        var objectArray = valueProvider.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim())).ToArray();
        // create an array of the accept type and same lenght 
        var guidArray = Array.CreateInstance(genericType, objectArray.Length); 
        // assign 
        objectArray.CopyTo(guidArray, 0);

        bindingContext.Model = guidArray;
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}