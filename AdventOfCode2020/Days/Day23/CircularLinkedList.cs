using System.Collections.Generic;

namespace AdventOfCode2020.Days.Day23
{
	public class CircularLinkedList<T>
	{
		public List<CircularLinkedListNode<T>> Nodes { get; } = new List<CircularLinkedListNode<T>>();

		public CircularLinkedListNode<T> First => Nodes[0];
		public CircularLinkedListNode<T> Last => Nodes[Nodes.Count - 1];

		public CircularLinkedList()
		{
			
		}

		public CircularLinkedList(List<T> values)
		{
			CircularLinkedListNode<T> first = new CircularLinkedListNode<T>(values[0]);
			CircularLinkedListNode<T> last = new CircularLinkedListNode<T>(values[values.Count - 1]);

			first.Next = last;
			first.Previous = last;
			last.Next = first;
			last.Previous = first;

			Nodes.Add(first);
			Nodes.Add(last);

			for (int i = 1; i < values.Count - 1; i++)
			{
				CircularLinkedListNode<T> newNode = new CircularLinkedListNode<T>(values[i]);
				Nodes.Add(newNode);
				AddAfter(last.Previous, newNode);
			}
		}

		public void AddAfter(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
		{
			newNode.Next = node.Next;
			newNode.Previous = node;

			node.Next.Previous = newNode;
			node.Next = newNode;
		}

		public void Remove(CircularLinkedListNode<T> node)
		{
			node.Next.Previous = node.Previous;
			node.Previous.Next = node.Next;
		}
	}
}