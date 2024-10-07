﻿using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProductApi.Model;
using static ProductAPI.Dtos.Dto;

namespace ProductApi.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private Connect conn = new();

        [HttpGet]
        public List<Product> Get()
        {
            List<Product> products = new();
            string sql = "SELECT * FROM products";

            conn.Connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            reader.Read();

            do
            {
                var result = new Product
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Price = reader.GetInt32(2),
                    CreatedTime = reader.GetDateTime(3)
                };

                products.Add(result);
            }
            while (reader.Read());

            conn.Connection.Close();

            return products;
        }

        [HttpPost]
        /*public object Post(Product product)
        {
            conn.Connection.Open();


            string sql = $"INSERT INTO `products`(`Id`, `Name`, `Price`, `CreatedTime`) VALUES ('{Guid.NewGuid()}','{product.Name}',{product.Price},'{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}')";

            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            cmd.ExecuteNonQuery();


            conn.Connection.Close();

            return new { message = "Új rekord felvéve!" };
        }*/
        public ActionResult<Product> Post(CreateProductDto product)
        {
            var result = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.name,
                Price = product.Price,
                CreatedTime=DateTime.Now,
            };
            string sql = $"INSERT INTO `products`(`id`, `Name`, `Price`, `CreatedTime`) VALUES ('{result.Id}','{result.Name}','{result.Price}','{result.CreatedTime}')";

            conn.Connection.Open();
            MySqlCommand cmd = new MySqlCommand( sql, conn.Connection);
            cmd.ExecuteNonQuery();
            conn.Connection.Close();
            return StatusCode(201, result);
        }

        [HttpPut]
        public ActionResult<Product> Put(Guid id, UpdateProductDto product) {
            string sql = $"UPDATE `products` SET `id`='{product.id}',`Name`='',`Price`='[{product.Price}]',`CreatedTime`='{product.CreatedTime}' WHERE 1";

            conn.Connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            cmd.ExecuteNonQuery();
            conn.Connection.Close();

                return Ok(new Product { Id=id,Name=product.name, Price=product.Price});
        }
        [HttpDelete]
        public ActionResult<object> Delete(Guid id) {
            string sql = $"DELETE FROM `products` WHERE Id= `{id}`";
            conn.Connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            cmd.ExecuteNonQuery();
            conn.Connection.Close();

            return Ok(new { message = "Sikeres törlés " });
        }
    }
}