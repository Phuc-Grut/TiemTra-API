using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class CategoryAttributes : BaseEntity
    {
        public int CategoryAttributeId { get; set; }
        public int CategoryId { get; set; }  // Liên kết với danh mục sản phẩm
        public int AttributeId { get; set; }  // Liên kết với thuộc tính

        public Category Category { get; set; }
        public Attributes Attribute { get; set; }
    }
}
