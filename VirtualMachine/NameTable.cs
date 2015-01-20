using System.Collections.Generic;

namespace VirtualMachine {
    internal class NameTable {
        private NameTable parent;

        private Dictionary<int, Name> names;

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

        public void Update(int key, Name name) {
            if (names.ContainsKey(key)) {
                names[key] = name;
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