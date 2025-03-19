using System;
using System.Collections.Generic;

namespace BlogEntity
{
    public class Post
    {
        public int PostId { get; set; }  // ID поста
        public string Title { get; set; } // Заголовок поста
        public string Content { get; set; } // Содержание поста
        public int BlogId { get; set; }   // Внешний ключ к Blog
        public Blog Blog { get; set; }    // Навигационное свойство для связи с блогом
    }
    public class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>(); // Коллекция постов
    }
}
