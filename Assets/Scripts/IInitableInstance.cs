using System;
using System.Collections.Generic;

public interface IInitableInstance {
    public void Init();

    public List<Type> GetDependencies() {
        return new List<Type>();
    }
}