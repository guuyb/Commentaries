using System.ComponentModel;

namespace System.Runtime.CompilerServices;

// fix CS0518 IsExternalInit is not defined or imported...
[EditorBrowsable(EditorBrowsableState.Never)]
internal class IsExternalInit { }
