using System.Dynamic;

dynamic obj = "hello";

Console.WriteLine($"{obj} {obj.GetType()}");

obj = 5;
obj += 10;

Console.WriteLine($"{obj} {obj.GetType()}");
Console.WriteLine(ReturnValue(true));
Console.WriteLine(ReturnValue(false));

PrintTypeValue(100);
PrintTypeValue("200");
PrintTypeValue(false);
PrintTypeValue(new List<string>());

dynamic employee = new ExpandoObject();
employee.Name = "Bob";
employee.Age = 24;
employee.Address = new { City = "Moscow", Street = "Tverskaya" };
employee.AddAge = (Action<int>)(age => employee.Age += age);
employee.AddAge(10);

dynamic myObj = new MyDynamicClass();

myObj.Name = "Tom";
myObj.Age = 24;
Func<int, int> addAge = (age) => myObj.Age += age;
myObj.AddAge = addAge;

myObj.AddAge(10);


dynamic ReturnValue(bool flag)
{
    if (flag)
        return 100;
    else
        return "200";
}

void PrintTypeValue(dynamic obj)
{
    Console.WriteLine($"{obj} {obj.GetType()}");
}

class MyDynamicClass : DynamicObject
{
    Dictionary<string, object> members = new();
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if(value is not null)
        {
            members[binder.Name] = value;
            return true;
        }
        return false;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = null;
        if(members.ContainsKey(binder.Name))
        {
            result = members[binder.Name];
            return true;
        }
        return false;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = null;
        if (args[0] is int argNum)
        {
            dynamic method = members[binder.Name];
            result = method(argNum);
        }
        return result != null;
    }
}