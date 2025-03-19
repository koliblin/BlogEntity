using Microsoft.EntityFrameworkCore;
using BlogEntity;
using System;
using System.Linq;
using System.IO;
// Убедитесь, что пространство имен соответствует вашему проекту


public class Program
{
    public static void Main(string[] args)
    {
        using (var context = new BloggingContext())
        {
            context.Database.EnsureCreated(); // Создаем базу данных, если ее нет

            // Сидирование данных (необязательно)
            if (!context.Blogs.Any())
            {
                var blog1 = new Blog { Title = "Первый блог", Url = "http://example.com/blog1" };
                var blog2 = new Blog { Title = "Второй блог", Url = "http://example.com/blog2" };

                context.Blogs.AddRange(blog1, blog2); // Add multiple blogs
                context.SaveChanges();

                // Add some posts to blog1
                context.Posts.Add(new Post { BlogId = blog1.BlogId, Title = "Post 1 for Blog 1", Content = "Content of Post 1" });
                context.Posts.Add(new Post { BlogId = blog1.BlogId, Title = "Post 2 for Blog 1", Content = "Content of Post 2" });
                context.SaveChanges();
            }

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Просмотр блогов");
                Console.WriteLine("2. Добавить блог");
                Console.WriteLine("3. Удалить блог");
                Console.WriteLine("4. Редактировать блог");
                Console.WriteLine("5. Просмотр постов блога"); // Added new option
                Console.WriteLine("6. Добавить пост к блогу");   // Added new option
                Console.WriteLine("7. Выход");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Просмотр блогов
                        var blogs = context.Blogs.ToList();
                        foreach (var blog in blogs)
                        {
                            Console.WriteLine($"ID: {blog.BlogId}, Title: {blog.Title}, URL: {blog.Url}");
                        }
                        break;

                    case "2":
                        // Добавление блога
                        Console.Write("Введите название блога: ");
                        var title = Console.ReadLine();
                        Console.Write("Введите URL блога: ");
                        var url = Console.ReadLine();
                        context.Blogs.Add(new Blog { Title = title, Url = url });
                        context.SaveChanges();
                        Console.WriteLine("Блог добавлен!");
                        break;

                    case "3":
                        // Удаление блога
                        Console.Write("Введите ID блога для удаления: ");
                        if (int.TryParse(Console.ReadLine(), out int blogIdToDelete))
                        {
                            var blogToDelete = context.Blogs.Find(blogIdToDelete);
                            if (blogToDelete != null)
                            {
                                context.Blogs.Remove(blogToDelete);
                                context.SaveChanges();
                                Console.WriteLine("Блог удален!");
                            }
                            else
                            {
                                Console.WriteLine("Блог не найден.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный ID.");
                        }
                        break;

                    case "4":
                        // Редактирование блога
                        Console.Write("Введите ID блога для редактирования: ");
                        if (int.TryParse(Console.ReadLine(), out int blogIdToUpdate))
                        {
                            var blogToUpdate = context.Blogs.Find(blogIdToUpdate);
                            if (blogToUpdate != null)
                            {
                                Console.Write("Введите новое название блога: ");
                                blogToUpdate.Title = Console.ReadLine();
                                Console.Write("Введите новый URL блога: ");
                                blogToUpdate.Url = Console.ReadLine();
                                context.SaveChanges();
                                Console.WriteLine("Блог обновлен!");
                            }
                            else
                            {
                                Console.WriteLine("Блог не найден.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный ID.");
                        }
                        break;

                    case "5":  // Просмотр постов блога
                        Console.Write("Введите ID блога для просмотра постов: ");
                        if (int.TryParse(Console.ReadLine(), out int blogIdToViewPosts))
                        {
                            var blog = context.Blogs.Include(b => b.Posts).FirstOrDefault(b => b.BlogId == blogIdToViewPosts);
                            if (blog != null)
                            {
                                Console.WriteLine($"Посты блога '{blog.Title}':");
                                foreach (var post in blog.Posts)
                                {
                                    Console.WriteLine($"  Post ID: {post.PostId}, Title: {post.Title}, Content: {post.Content}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Блог не найден.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный ID.");
                        }
                        break;

                    case "6":  // Добавление поста к блогу
                        Console.Write("Введите ID блога, к которому добавить пост: ");
                        if (int.TryParse(Console.ReadLine(), out int blogIdToAddPost))
                        {
                            var blog = context.Blogs.Find(blogIdToAddPost);
                            if (blog != null)
                            {
                                Console.Write("Введите заголовок поста: ");
                                var postTitle = Console.ReadLine();
                                Console.Write("Введите содержание поста: ");
                                var postContent = Console.ReadLine();

                                var newPost = new Post { BlogId = blogIdToAddPost, Title = postTitle, Content = postContent };
                                context.Posts.Add(newPost);
                                context.SaveChanges();
                                Console.WriteLine("Пост добавлен!");
                            }
                            else
                            {
                                Console.WriteLine("Блог не найден.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный ID.");
                        }
                        break;

                    case "7":
                        // Выход
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}