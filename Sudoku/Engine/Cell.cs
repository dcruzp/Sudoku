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
        public Cell()
        {
            this.type = typecell.empty; 
        }

        public Cell (typecell typecell )
        {
            this.type = typecell; 
        }

        public typecell Type
        {
            get => this.type;
            set => this.type = value; 
        }
    }
}
