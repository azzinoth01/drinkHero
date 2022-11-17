using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface ICascadable {


    public List<ICascadable> Cascadables {
        get;
        set;
    }

    public void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null);


}
