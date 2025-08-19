using System;
using System.Collections.Generic;
using AI.Node.Conditions;

namespace AI.Node.Jobs {
	public class Job_Search : Sequence {
		public Job_Search(Settler settler) { 
			var data = settler.Data;
			Func<bool> condition = () => data.currTarget &&
			                             data.currTarget.Data.CurrentJob == JobType.Search &&
			                             data.currJob == JobType.Search;
			
			var move = new ConditionalAction()
				.Do(new Action_MoveTo(settler))
				.While(condition);
			var search = new ConditionalAction()
				.Do(new Action_SearchObject(settler))
				.While(condition);
			
			AddChild(move);
			AddChild(search);
		}
	}
}