using UnityEngine;

[System.Serializable]
public class InventoryItemContainer
{
	[SerializeField] private InventoryItem item;
	[SerializeField] private int count;

	public InventoryItem getItem()
	{
		return item;
	}

	public int getItemCount()
	{
		return count;
	}
}
