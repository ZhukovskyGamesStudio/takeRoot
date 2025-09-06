using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITacticalService : IService {
	public Action OnTacticalSwitched { get; set; }
	public void SetTacticalForSelectedSettlers();
}