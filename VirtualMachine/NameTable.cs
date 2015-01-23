using System.Collections.Generic;
using System.Linq;

namespace Speedycloud.Runtime {
    public class NameTable {
        public NameTable Parent { get { return parent; } }
        private readonly NameTable parent;

        private readonly Dictionary<int, Name> names = new Dictionary<int, Name>();

        public NameTable() {
        }
        public NameTable(NameTable parent) {
            this.parent = parent;
        }

        public Name Lookup(int key) {
            if (names.ContainsKey(key)) {
                return names[key];
            }
            if (parent == null) {
                throw new RuntimeException("Name accessed, but name is not in the name table");
            }
            return parent.Lookup(key);
        }

        public bool ContainsReferenceTo(int id) {
            if (names.Any(name => name.Value.Value == id)) {
                return true;
            }

            if (parent == null) {
                return false;
            }
            return parent.ContainsReferenceTo(id);
        }

        public void Update(int key, Name name) {
            if (names.ContainsKey(key)) {
                names[key] = name;
                return;
            }
            if (parent == null) {
                throw new RuntimeException("Name updated, but name is not in the name table");
            }
            parent.Update(key, name);
        }

        public void New(int key, Name name) {
            if (names.ContainsKey(key)) {
                throw new RuntimeException("Name initialised, but name already exists in scope");
            }
            names[key] = name;
        }
    }
}