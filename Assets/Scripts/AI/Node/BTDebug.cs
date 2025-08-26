using System.Collections.Generic;

public static class BTDebug {
    private static readonly Stack<string> _context = new();
    public static bool Enabled = true;

    public static void Enter(string nodeName) {
        if (!Enabled) {
            return;
        }

        _context.Push(nodeName);
    }

    public static void Exit() {
        if (!Enabled) {
            return;
        }

        if (_context.Count > 0) {
            _context.Pop();
        }
    }

    public static void Log(string message) {
        if (!Enabled) {
            return;
        }
        //Debug.Log($"[BT] {string.Join(" > ", _context)} : {message}");
    }
}