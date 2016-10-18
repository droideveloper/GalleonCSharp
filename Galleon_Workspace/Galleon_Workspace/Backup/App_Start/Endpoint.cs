using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using GallonEntity;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace GallonEndpoint {
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Endpoint : IEndpoint {

		private const string PROPERTY_NAME 		= "name";
		private const string PROPERTY_SURNAME 	= "surname";
		private const string PROPERTY_IDENTITY 	= "identity";

		public const string SESSION_TOKEN = "X-Auth-Token";
		private GallonEntityContext entityContext = new GallonEntityContext();

		public Response<Contact> createContact(Contact contact) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Contact>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
			                               .Where(s => s.Token.Equals(token))
			                               .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) { 
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Contact>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(entityContext.Contacts.Contains(contact)) {
				return new Response<Contact>() { 
					Code = 404,
					Message = "contact exits, try update.",
					Data = null
				};
			}
			//save and return
			entityContext.Contacts.Add(contact);
			entityContext.SaveChanges();

			return new Response<Contact>() {
				Code = 200,
				Message = "success.",
				Data = contact
			};
		}

		public Response<Customer> createCustomer(Customer customer) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Customer>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Customer>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(entityContext.Customers.Contains(customer)) {
				return new Response<Customer>() {
					Code = 404,
					Message = "customer exists, try update.",
					Data = null
				};
			}
			//save
			entityContext.Customers.Add(customer);
			entityContext.SaveChanges();

			return new Response<Customer>() {
				Code = 200,
				Message = "success.",
				Data = customer
			};
		}

		public Response<Directory> createDirectory(Directory directory) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Directory>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Directory>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(entityContext.Directories.Contains(directory)) {
				return new Response<Directory>() {
					Code = 404,
					Message = "directory exists, try update.",
					Data = null
				};
			}
			//save
			entityContext.Directories.Add(directory);
			entityContext.SaveChanges();

			return new Response<Directory>() {
				Code = 200,
				Message = "success.",
				Data = directory
			};
		}

		public Response<Document> createDocument(Document document) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Document>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Document>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(entityContext.Documents.Contains(document)) {
				return new Response<Document>() {
					Code = 404,
					Message = "document exists, try update.",
					Data = null
				};
			}
			//save
			entityContext.Documents.Add(document);
			entityContext.SaveChanges();

			return new Response<Document>() {
				Code = 200,
				Message = "success.",
				Data = document
			};
		}

		public Response<bool> deleteContact(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<bool>() {
					Code = 401,
					Message = "unauthorized.",
					Data = false
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<bool>() {
					Code = 401,
					Message = "session expired.",
					Data = false
				};
			}
			int contactID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out contactID)) {
				return new Response<bool>() {
					Code = 404,
					Message = "inalid id.",
					Data = false
				};
			}
			//find it
			Contact contact = entityContext.Contacts
										   .Where(c => c.ContactID == contactID)
										   .FirstOrDefault();
			if(contact != null) { 
				entityContext.Contacts.Remove(contact);
				entityContext.SaveChanges();
			}

			return new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = true
			};
		}

		public Response<bool> deleteCustomer(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<bool>() {
					Code = 401,
					Message = "unauthorized.",
					Data = false
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<bool>() {
					Code = 401,
					Message = "session expired.",
					Data = false
				};
			}
			//try parse
			int customerID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
				return new Response<bool>() {
					Code = 404,
					Message = "invalid id.",
					Data = false
				};
			}
			//find customer
			Customer customer = entityContext.Customers
			                                 .Where(c => c.CustomerID == customerID)
			                                 .FirstOrDefault();
			if(customer != null) { 
				entityContext.Customers.Remove(customer);
				entityContext.SaveChanges();
			}

			return new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = true
			};
		}

		public Response<bool> deleteDirectory(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<bool>() {
					Code = 401,
					Message = "unauthorized.",
					Data = false
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<bool>() {
					Code = 401,
					Message = "session expired.",
					Data = false
				};
			}
			//try parse
			int directoryID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out directoryID)) {
				return new Response<bool>() {
					Code = 404,
					Message = "invalid id.",
					Data = false
				};
			}
			//find directory
			Directory directory = entityContext.Directories
			                                   .Where(d => d.DirectoryID == directoryID)
			                                   .FirstOrDefault();
			if(directory != null) { 
				entityContext.Directories.Remove(directory);
				entityContext.SaveChanges();
			}

			return new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = false
			};
		}

		public Response<bool> deleteDocument(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<bool>() {
					Code = 401,
					Message = "unauthorized.",
					Data = false
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<bool>() {
					Code = 401,
					Message = "session expired.",
					Data = false
				};
			}
			//try parse
			int documentID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out documentID)) {
				return new Response<bool>() {
					Code = 404,
					Message = "invalid id.",
					Data = false
				};
			}
			//find document
			Document document = entityContext.Documents
			                                 .Where(d => d.DocumentID == documentID)
			                                 .FirstOrDefault();
			if(document != null) { 
				entityContext.Documents.Remove(document);
				entityContext.SaveChanges();
			}

			return new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = true
			};
		}

		public Response<Session> keepAlive(string token) {
			if(String.IsNullOrEmpty(token)) {
				return new Response<Session>() {
					Code = 404,
					Message = "invalid request.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
			                               .Where(s => s.Token.Equals(token))
			                               .FirstOrDefault();
			if(session == null) {
				return new Response<Session>() {
					Code = 404,
					Message = "no such session exists, try sign in.",
					Data = null
				};
			}
			//update session
			session.DueDate = DateTime.Now.AddMinutes(30);
			entityContext.SaveChanges();

			return new Response<Session>() {
				Code = 200,
				Message = "success.",
				Data = session
			};
		}

		public Response<List<Customer>> queryAllCustomers() {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

			List<Customer> customers = entityContext.Customers.ToList();
			return new Response<List<Customer>>() {
				Code = 200,
				Message = "success.",
				Data = customers
			};
		}

		public Response<Contact> queryContact(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Contact>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Contact>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//try parse
			int contactID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out contactID)) {
				return new Response<Contact>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Contact contact = entityContext.Contacts
			                               .Where(c => c.ContactID == contactID)
			                               .FirstOrDefault();

			return new Response<Contact>() {
				Code = 200,
				Message = "success.",
				Data = contact
			};
		}

		public Response<List<Contact>> queryContactsByCustomerID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Contact>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Contact>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//try parse
			int customerID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
				return new Response<List<Contact>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Contact> contacts = entityContext.Contacts
			                                      .Where(c => c.CustomerID == customerID)
										   		  .ToList();

			return new Response<List<Contact>>() {
				Code = 200,
				Message = "success.",
				Data = contacts
			};
		}

		public Response<List<Contact>> queryContactsByCustomerIDAndStartAndLimit(string id, string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Contact>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(se => se.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Contact>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID, s, l;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
				return new Response<List<Contact>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			int.TryParse(start, out s);
			int.TryParse(limit, out l);

			List<Contact> contacts = entityContext.Contacts
			                                      .Where(c => c.CustomerID == customerID)
			                                      .Skip(s >= 0 ? s : 0)//just to be safe
			                                      .Take(l >= 0 ? l : 0)//just to be safe
			                                      .ToList();

			return new Response<List<Contact>>() {
				Code = 200,
				Message = "success.",
				Data = contacts
			};
		}

		public Response<Customer> queryCustomer(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Customer>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Customer>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

			int customerID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
				return new Response<Customer>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Customer customer = entityContext.Customers
			                                 .Where(c => c.CustomerID == customerID)
			                                 .FirstOrDefault();
			//return response
			return new Response<Customer>() {
				Code = 200,
				Message = "success.",
				Data = customer
			};
		}

		public Response<List<Customer>> queryCustomersWithStartAndLimit(string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(se => se.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int s, l;
			int.TryParse(start, out s);
			int.TryParse(limit, out l);

			List<Customer> customers = entityContext.Customers
			                                        .Skip(s >= 0 ? s : 0)
			                                        .Take(l >= 0 ? l : 0)
			                                        .ToList();

			return new Response<List<Customer>>() {
				Code = 200,
				Message = "success.",
				Data = customers
			};
		}

		public Response<List<Customer>> queryCustomerWithQueryAndProperty(string query, string property) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Customer>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			if(String.IsNullOrEmpty(query) || String.IsNullOrEmpty(property)) {
				return new Response<List<Customer>>() {
					Code = 404,
					Message = "invalid query and property."
				};
			}

			switch(property) {
				case PROPERTY_NAME: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Firstname.Contains(query))
						                                        .ToList();
						return new Response<List<Customer>>() {
							Code = 200,
							Message = "success.",
							Data = customers
						};
					}
				case PROPERTY_SURNAME: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Lastname.Contains(query))
																.ToList();
						return new Response<List<Customer>>() {
							Code = 200,
							Message = "success.",
							Data = customers
						}; 
					}
				case PROPERTY_IDENTITY: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Identity.Contains(query))
																.ToList();
						return new Response<List<Customer>>() {
							Code = 200,
							Message = "success.",
							Data = customers
						}; 
					}
				default:
					return new Response<List<Customer>>() {
						Code = 404,
						Message = "unsupported property.",
						Data = null
					};
			}
		}

		public Response<List<Directory>> queryDirectoriesByCustomerID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Directory>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Directory>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID;
			if(String.IsNullOrEmpty(id) || int.TryParse(id, out customerID)) {
				return new Response<List<Directory>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Directory> directories = entityContext.Directories
			                                           .Where(d => d.CustomerID == customerID)
			                                           .ToList();

			return new Response<List<Directory>>() {
				Code = 200,
				Message = "success.",
				Data = directories
			};
		}

		public Response<List<Directory>> queryDirectoriesByCustomerIDAndStartAndLimit(string id, string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Directory>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(se => se.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<List<Directory>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID, s, l;
			if(String.IsNullOrEmpty(id) || int.TryParse(id, out customerID)) {
				return new Response<List<Directory>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			int.TryParse(start, out s);
			int.TryParse(limit, out l);
			List<Directory> directories = entityContext.Directories
													   .Where(d => d.CustomerID == customerID)
			                                           .Skip(s >= 0 ? s : 0)
			                                           .Take(l >= 0 ? l : 0)
													   .ToList();

			return new Response<List<Directory>>() {
				Code = 200,
				Message = "success.",
				Data = directories
			};
		}

		public Response<Directory> queryDirectory(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Directory>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Directory>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int directoryID;
			if(String.IsNullOrEmpty(id) || int.TryParse(id, out directoryID)) {
				return new Response<Directory>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Directory directory = entityContext.Directories
			                                   .Where(d => d.DirectoryID == directoryID)	
			                                   .FirstOrDefault();

			return new Response<Directory>() {
				Code = 200,
				Message = "success.",
				Data = directory
			};
		}

		public Response<Document> queryDocument(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Document>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Document>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int documentID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out documentID)) {
				return new Response<Document>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}

			Document document = entityContext.Documents
			                                 .Where(d => d.DocumentID == documentID)
			                                 .FirstOrDefault();

			return new Response<Document>() {
				Code = 200,
				Message = "success.",
				Data = document
			};
		}

		public Response<List<Document>> queryDocumentsByDirectoryAndStartAndLimit(string id, string start, string limit) {
			throw new NotImplementedException();
		}

		public Response<List<Document>> queryDocumentsByDirectoryID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<List<Document>>() { 
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}

			Session sesssion = entityContext.Sessions
			                                .Where(s => s.Token.Equals(token))
			                                .FirstOrDefault();
			if(sesssion == null || sesssion.DueDate < DateTime.Now) {
				if(sesssion != null) { 
					entityContext.Sessions.Remove(sesssion);
					entityContext.SaveChanges();
				}
				return new Response<List<Document>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			int directoryID;
			if(String.IsNullOrEmpty(id) || !int.TryParse(id, out directoryID)) {
				return new Response<List<Document>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Document> documents = entityContext.Documents
			                                        .Where(d => d.DirectoryID == directoryID)
			                                        .ToList();
			return new Response<List<Document>>() { 
				Code = 200,
				Message = "success.",
				Data = documents
			};
		}

		public Response<Session> signin(Sign sign) {
			if(String.IsNullOrEmpty(sign.UserName) || String.IsNullOrEmpty(sign.Password)) {
				return new Response<Session>() {
					Code = 404,
					Message = "specify username or password.",
					Data = null
				};
			}
			User user = entityContext.Users
									 .Where(s => s.Username.Equals(sign.UserName)
												  && s.Password.Equals(sign.Password))
									 .FirstOrDefault();
			if(user == null) {
				return new Response<Session>() {
					Code = 404,
					Message = "username or password is invalid.",
					Data = null
				};
			}

			Session session = new Session() {
				UserID = user.UserID,
				Token = Token.CreateToken(),
				CreateDate = DateTime.Now,
				DueDate = DateTime.Now.AddMinutes(30)
			};
			//store session
			entityContext.Sessions.Add(session);
			entityContext.SaveChanges();

			return new Response<Session>() {
				Code = 200,
				Message = "success.",
				Data = session
			};
		}

		public Response<bool> signout(string token) {
			if(String.IsNullOrEmpty(token)) {
				return new Response<bool>() {
					Code = 404,
					Message = "invalid request.",
					Data = false
				};
			}
			Session session = entityContext.Sessions
			                               .Where(s => s.Token.Equals(token))
			                               .FirstOrDefault();
			if(session == null) {
				return new Response<bool>() {
					Code = 404,
					Message = "invalid session.",
					Data = false
				};
			}
			//remove user session
			entityContext.Sessions.Remove(session);
			entityContext.SaveChanges();

			return new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = true
			};
		}

		public Response<Contact> updateContact(Contact contact) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Contact>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Contact>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(!entityContext.Contacts.Contains(contact)) {
				return new Response<Contact>() {
					Code = 404,
					Message = "contact not exists, try create.",
					Data = null
				};
			}
			//save
			entityContext.Contacts.Attach(contact);
			entityContext.Entry(contact).State = EntityState.Modified;
			entityContext.SaveChanges();

			return new Response<Contact>() {
				Code = 200,
				Message = "success.",
				Data = contact
			};
		}

		public Response<Customer> updateCustomer(Customer customer) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Customer>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Customer>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(!entityContext.Customers.Contains(customer)) {
				return new Response<Customer>() {
					Code = 404,
					Message = "customer not exists, try create.",
					Data = null
				};
			}
			//save
			entityContext.Customers.Attach(customer);
			entityContext.Entry(customer).State = EntityState.Modified;
			entityContext.SaveChanges();

			return new Response<Customer>() {
				Code = 200,
				Message = "success.",
				Data = customer
			};
		}

		public Response<Directory> updateDirectory(Directory directory) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Directory>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Directory>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(!entityContext.Directories.Contains(directory)) {
				return new Response<Directory>() {
					Code = 404,
					Message = "directory not exists, try create.",
					Data = null
				};
			}
			//save
			entityContext.Directories.Attach(directory);
			entityContext.Entry(directory).State = EntityState.Modified;
			entityContext.SaveChanges();

			return new Response<Directory>() {
				Code = 200,
				Message = "success.",
				Data = directory
			};		
		}

		public Response<Document> updateDocument(Document document) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(String.IsNullOrEmpty(token)) {
				return new Response<Document>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
										   .Where(s => s.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				//clear expired session
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				return new Response<Document>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
			if(!entityContext.Documents.Contains(document)) {
				return new Response<Document>() {
					Code = 404,
					Message = "directory not exists, try create.",
					Data = null
				};
			}
			//save
			entityContext.Documents.Attach(document);
			entityContext.Entry(document).State = EntityState.Modified;
			entityContext.SaveChanges();

			return new Response<Document>() {
				Code = 200,
				Message = "success.",
				Data = document
			};
		}
	}

	class Token {
		
		public static string CreateToken() {
			StringBuilder str = new StringBuilder();
			Guid.NewGuid()
				.ToByteArray()
				.ToList()
				.ForEach(b => str.Append(b.ToString("X2")));
			return str.ToString();
		}

		public static string GetToken(IncomingWebRequestContext request) {
			string[] keys = request.Headers.AllKeys;
			string[] values;
			for(int i = 0, z = keys.Length; i < z; i++) {
				string key = keys[i];
				if(Endpoint.SESSION_TOKEN.Equals(key)) {
					values = request.Headers.GetValues(key);
					return values.ToList().FirstOrDefault();
				}
			}
			return null;
		}
	}
}

