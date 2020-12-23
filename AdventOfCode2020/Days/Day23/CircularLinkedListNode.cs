namespace AdventOfCode2020.Days.Day23
{
	public class CircularLinkedListNode<T>
	{
		public T Value { get; set; }
		public CircularLinkedListNode<T> Next { get; set; }
		public CircularLinkedListNode<T> Previous { get; set; }

		public CircularLinkedListNode(T value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}