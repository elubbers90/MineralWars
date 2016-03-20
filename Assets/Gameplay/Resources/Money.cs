using System.Collections.Generic;

public enum ResourceType{
	GOLD = 0,
	BLUE = 1,
	GREEN = 2,
	ORANGE = 3,
	PINK = 4,
	PURPLE = 5,
	RED = 6
}

public class Money
{
	private Dictionary<ResourceType,int> resources;

	public Money (int gold=0,int blue=0,int green=0,int orange=0,int pink=0,int purple=0,int red=0)
	{
		resources = new Dictionary<ResourceType, int> ();
		resources.Add (ResourceType.GOLD, gold);
		resources.Add (ResourceType.BLUE, blue);
		resources.Add (ResourceType.GREEN, green);
		resources.Add (ResourceType.ORANGE, orange);
		resources.Add (ResourceType.PINK, pink);
		resources.Add (ResourceType.PURPLE, purple);
		resources.Add (ResourceType.RED, red);
	}

	public int GetResource(ResourceType resource){
		int result;
		if (resources.TryGetValue (resource, out result)) {
			return result;
		}
		return 0;
	}

	public void AddResource(ResourceType resource, int amount){
		int result;
		if (resources.TryGetValue (resource, out result)) {
			resources[resource] = result + amount;
		}
		else{
			resources.Add(resource, amount);
		}
	}

	public Dictionary<ResourceType,int> GetResources(){
		return resources;
	}
}

