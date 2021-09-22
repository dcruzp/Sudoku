using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine
{
    class Cell 
    {
        typecell type;
        bool badCell; 

        public Cell():this(typecell.empty)
        {
        }

        public Cell (typecell typecell )
        {
            this.type = typecell;
            this.badCell = false; 
        }

        public typecell Type
        {
            get => this.type;
            set => this.type = value; 
        }

        public bool BadCell
        {
            get => this.badCell;
            set => this.badCell = value; 
        }
    }
}
