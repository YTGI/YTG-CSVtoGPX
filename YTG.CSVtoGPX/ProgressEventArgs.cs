// --------------------------------------------------------------------------------
/*  Copyright © 2020, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: EventArgs used to report progress

    Description: 

*/
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace YTG.CSVtoGPX
{
    public class ProgressEventArgs
    {
        string m_description = string.Empty;
        long m_id = 0;

        public long id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public ProgressEventArgs(string p_Description)
        {
            this.Description = p_Description;
            this.id = DateTime.Now.ToFileTime();
        }

        public ProgressEventArgs(long p_Id, string p_Description)
        {
            this.Description = p_Description;
            this.id = p_Id;
        }

    }

}
