using System.Collections.Generic;
using System.Linq;

namespace MemberRMS.Controllers
{
    public class CategoriesRepository
    {
        private CategoriesController _categoriesController;
        private List<int> idsList = new List<int>();
        
        public CategoriesRepository(CategoriesController categoriesController)
        {
            _categoriesController = categoriesController;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoriesController.db.Category.ToList().AsEnumerable();
        }

        public void SetNameById(int id, string name)
        {
            var node = _categoriesController.db.Category.FirstOrDefault(x => x.CategoryID == id);
            if (node != null)
                node.Title = name;
            _categoriesController.db.SaveChanges();
        }

        public void RemoveById(int id)
        {
            var rootNode = _categoriesController.db.Category.FirstOrDefault(x => x.CategoryID == id);
            if (rootNode == null)
            { return; }
            DeleteSubNodes(rootNode);
            idsList.Add(rootNode.CategoryID);
            foreach (int i in idsList)
            {
                _categoriesController.db.Category.Remove(_categoriesController.db.Category.FirstOrDefault(x => x.CategoryID == i));
                _categoriesController.db.SaveChanges();
            }
            idsList.Clear();
        }

        public void Move(Category node)
        {
            var tempNode = _categoriesController.db.Category.FirstOrDefault(x => x.CategoryID == node.CategoryID);
            if (tempNode != null)
            {
                tempNode.ParentCategoryID = node.ParentCategoryID;
                _categoriesController.db.SaveChanges();
            }
        }

        public void _AddNode(Category node)
        {
            var id = _categoriesController.db.Category.Max(x => x.CategoryID);

            _categoriesController.db.Category.Add(new Category()
            {
                CategoryID = id+1,
                ParentCategoryID = node.ParentCategoryID,
                Title = node.Title
            });
            _categoriesController.db.SaveChanges();
        }

        public int GetIdBy(Category node)
        {
            var reqNode = _categoriesController.db.Category.Where(x => x.ParentCategoryID == node.ParentCategoryID)
                .FirstOrDefault(y => y.Title == node.Title);
            if (reqNode != null)
                return reqNode.CategoryID;
            return 0;
        }

        private void DeleteSubNodes(Category node)
        {
            var selNodes = _categoriesController.db.Category.Where(x => x.ParentCategoryID == node.CategoryID).ToList();
            if (!selNodes.Any())
            {
                return;
            }

            foreach (var selNode in selNodes)
            {
                Category childNode = new Category();
                childNode.CategoryID = selNode.CategoryID;
                idsList.Add(childNode.CategoryID);
                DeleteSubNodes(childNode);
            }
        }

        public IEnumerable<Category> GetChildrenNodes(int id)
        {
            var nodes = _categoriesController.db.Category.Where(x => x.ParentCategoryID == id);
            return nodes;
        }
    }
}