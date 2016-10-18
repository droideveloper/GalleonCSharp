using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using GalleonEntity;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ServiceModel;
using System.ServiceModel.Web;
using GalleonEndpoint.Proxy;

namespace GalleonEndpoint {

	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Endpoint : IEndpoint {

		private const string PROPERTY_NAME 		= "name";
		private const string PROPERTY_SURNAME 	= "surname";
		private const string PROPERTY_IDENTITY 	= "identity";

		public const string SESSION_TOKEN = "X-Auth-Token";
		private GalleonEntityContext entityContext = new GalleonEntityContext();

        public Response<ContactProxy> createContact(ContactProxy newContact) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<ContactProxy>() {
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
                return new Response<ContactProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

            Contact contact = entityContext.Contacts.Where(c => c.ContactID == newContact.CountactID).FirstOrDefault();
            if(contact != null) { 
                return new Response<ContactProxy>() {
                    Code = 404,
                    Message = "try update, already exists.",
                    Data = null
                };
            }

            contact = new Contact() {
                CustomerID = newContact.CustomerID,
                ContactName = newContact.ContactName,
                Address = newContact.Address,
                Phone = newContact.Phone,
                CityID = newContact.CityID,
                CountryID = newContact.CountryID,
                CreateDate = DateTime.Now,
                UpdateDate  = DateTime.Now
            };

            entityContext.Contacts.Add(contact);
            entityContext.SaveChanges();

            entityContext.Entry(contact).Reference(p => p.City).Load();
            entityContext.Entry(contact).Reference(p => p.Country).Load();

            return new Response<ContactProxy>() {
                Code = 200,
                Message = "success.",
                Data = new ContactProxy(contact)
            };
		}

        public Response<List<ContactProxy>> createContacts(List<ContactProxy> contacts) {
            try {
                string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
                if(string.IsNullOrEmpty(token)) {
                    return new Response<List<ContactProxy>>() {
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
                    return new Response<List<ContactProxy>>() {
                        Code = 401,
                        Message = "session expired.",
                        Data = null
                    };
                }

                if(contacts.IsNullOrEmpty()) {
                    return new Response<List<ContactProxy>>() {
                        Code = 404,
                        Message = "No contact to create.",
                        Data = null
                    };
                }

                List<Contact> conts = contacts.Select(c =>
                    new Contact() {
                        CustomerID = c.CustomerID,
                        ContactName = c.ContactName,
                        Address = c.Address,
                        Phone = c.Phone,
                        CityID = c.CityID,
                        CountryID = c.CountryID,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    }).ToList();

                entityContext.Contacts.AddRange(conts);
                entityContext.SaveChanges();

                conts.ForEach(x => {
                    x.City = entityContext.Cities.FirstOrDefault(y => y.CityID == x.CityID);
                    x.Country = entityContext.Countries.FirstOrDefault(y => y.CountryID == x.CountryID);
                });

                return new Response<List<ContactProxy>>() {
                    Code = 200,
                    Message = "success.",
                    Data = ContactProxy.FromContacts(conts)
                };
            } catch(Exception e) {
                return new Response<List<ContactProxy>>() {
                    Code = 404,
                    Message = e.Message + "\n" + e.StackTrace,
                    Data = null
                };
            }
        }

        public Response<CustomerProxy> createCustomer(CustomerProxy newCustomer) {
            try {
                string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
                if(string.IsNullOrEmpty(token)) {
                    return new Response<CustomerProxy>() {
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
                    return new Response<CustomerProxy>() {
                        Code = 401,
                        Message = "session expired.",
                        Data = null
                    };
                }

                Customer customer = entityContext.Customers
                                                 .Where(c => c.CustomerID == newCustomer.CustomerID)
                                                 .FirstOrDefault();

                if(customer != null) {
                    return new Response<CustomerProxy>() {
                        Code = 404,
                        Message = "already exits, try update.",
                        Data = null
                    };
                }

                customer = new Customer() {
                    Firstname = newCustomer.FirstName,
                    Lastname = newCustomer.LastName,
                    Identity = newCustomer.Identity,
                    CategoryID = newCustomer.CategoryID,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                entityContext.Customers.Add(customer);
                entityContext.SaveChanges();

                entityContext.Entry(customer).Reference(p => p.Category).Load();

                return new Response<CustomerProxy>() {
                    Code = 200,
                    Message = "success.",
                    Data = new CustomerProxy(customer)
                };
            } catch(Exception e) {
                return new Response<CustomerProxy>() {
                    Code = 404,
                    Message = e.StackTrace,
                    Data = null
                };
            }
		}

        public Response<DirectoryProxy> createDirectory(DirectoryProxy newDirectory) {
            try {
                string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
                if(string.IsNullOrEmpty(token)) {
                    return new Response<DirectoryProxy>() {
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
                    return new Response<DirectoryProxy>() {
                        Code = 401,
                        Message = "session expired.",
                        Data = null
                    };
                }

                Directory directory = entityContext.Directories
                                                   .Where(d => d.DirectoryID == newDirectory.DirectoryID)
                                                   .FirstOrDefault();

                if(directory != null) {
                    return new Response<DirectoryProxy>() {
                        Code = 404,
                        Message = "directory exists, try update.",
                        Data = null
                    };
                }

                directory = new Directory() {
                    DirectoryName = newDirectory.DirectoryName,
                    ParentDirectoryID = newDirectory.ParentDirectoryID,
                    CustomerID = newDirectory.CustomerID,
                    UserID = session.UserID,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                entityContext.Directories.Add(directory);
                entityContext.SaveChanges();

                return new Response<DirectoryProxy>() {
                    Code = 200,
                    Message = "success.",
                    Data = new DirectoryProxy(directory)
                };
            } catch(Exception e) {
                return new Response<DirectoryProxy>() {
                    Code = 404,
                    Message = e.Message + "\n" + e.StackTrace,
                    Data = null
                };
            }
		}

        public Response<DocumentProxy> createDocument(DocumentProxy newDocument) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<DocumentProxy>() {
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
                return new Response<DocumentProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

            Document document = entityContext.Documents
                                             .Where(d => d.DocumentID == newDocument.DocumentID)
                                             .FirstOrDefault();

            if(document != null) {
                return new Response<DocumentProxy>() { 
                    Code = 404,
                    Message = "already exists, try update.",
                    Data = null
                };
            }

            document = new Document() {
                DocumentName = newDocument.DocumentName,
                DirectoryID = newDocument.DirectoryID,
                ContentType = newDocument.ContentType,
                ContentLength = newDocument.ContentLength,
                UserID = session.UserID,
                CustomerID = newDocument.CustomerID,
                CreateDate = newDocument.CreateDate,
                UpdateDate = newDocument.UpdateDate
            };

            entityContext.Documents.Add(document);
            entityContext.SaveChanges();

            return new Response<DocumentProxy>() {
                Code = 200,
                Message = "success.",
                Data = new DocumentProxy(document)
            };
		}

        public Response<List<DocumentProxy>> createDocuments(List<DocumentProxy> documents) {
            try {
                string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
                if(string.IsNullOrEmpty(token)) {
                    return new Response<List<DocumentProxy>>() {
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
                    return new Response<List<DocumentProxy>>() {
                        Code = 401,
                        Message = "session expired.",
                        Data = null
                    };
                }

                if(documents.IsNullOrEmpty()) {
                    return new Response<List<DocumentProxy>>() {
                        Code = 404,
                        Message = "documents are null or empty.",
                        Data = null
                    };
                }

                List<Document> docs = documents.Select(x =>
                    new Document() {
                        DocumentName = x.DocumentName,
                        DirectoryID = x.DirectoryID,
                        ContentType = x.ContentType,
                        ContentLength = x.ContentLength,
                        UserID = session.UserID,
                        CustomerID = x.CustomerID,
                        CreateDate = x.CreateDate,
                        UpdateDate = x.UpdateDate
                    }).ToList();

                entityContext.Documents.AddRange(docs);
                entityContext.SaveChanges();
                //we are good
                return new Response<List<DocumentProxy>>() {
                    Code = 200,
                    Message = "success.",
                    Data = DocumentProxy.FromDocuments(docs)
                };
            } catch(Exception e) {
                return new Response<List<DocumentProxy>>() {
                    Code = 404,
                    Message = e.Message + "\n" + e.StackTrace,
                    Data = null
                };
            }
        }


		public Response<bool> deleteContact(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
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
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out contactID)) {
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
			if(string.IsNullOrEmpty(token)) {
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
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
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
			if(string.IsNullOrEmpty(token)) {
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
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out directoryID)) {
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
			if(string.IsNullOrEmpty(token)) {
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
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out documentID)) {
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

        public Response<SessionProxy> keepAlive(string token) {
			if(string.IsNullOrEmpty(token)) {
                return new Response<SessionProxy>() {
					Code = 404,
					Message = "invalid request.",
					Data = null
				};
			}
			Session session = entityContext.Sessions
			                               .Where(s => s.Token.Equals(token))
			                               .FirstOrDefault();
			if(session == null) {
                return new Response<SessionProxy>() {
					Code = 404,
					Message = "no such session exists, try sign in.",
					Data = null
				};
			}
			//update session
			session.DueDate = DateTime.Now.AddMinutes(30);
			entityContext.SaveChanges();

            return new Response<SessionProxy>() {
                Code = 200,
                Message = "success.",
                Data = new SessionProxy() { Token = session.Token }
            };
		}

        public Response<List<CustomerProxy>> queryAllCustomers() {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<CustomerProxy>>() {
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
                return new Response<List<CustomerProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

			List<Customer> customers = entityContext.Customers.ToList();
            return new Response<List<CustomerProxy>>() {
				Code = 200,
				Message = "success.",
				Data = CustomerProxy.FromCustomers(customers)
			};
		}

		public Response<ContactProxy> queryContact(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<ContactProxy>() {
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
                return new Response<ContactProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//try parse
			int contactID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out contactID)) {
                return new Response<ContactProxy>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Contact contact = entityContext.Contacts
			                               .Where(c => c.ContactID == contactID)
			                               .FirstOrDefault();

            return new Response<ContactProxy>() {
				Code = 200,
				Message = "success.",
				Data = new ContactProxy(contact)
			};
		}

        public Response<List<ContactProxy>> queryContactsByCustomerID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<ContactProxy>>() {
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
                return new Response<List<ContactProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//try parse
			int customerID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
                return new Response<List<ContactProxy>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Contact> contacts = entityContext.Contacts
			                                      .Where(c => c.CustomerID == customerID)
										   		  .ToList();

            return new Response<List<ContactProxy>>() {
				Code = 200,
				Message = "success.",
				Data = ContactProxy.FromContacts(contacts)
			};
		}

        public Response<List<ContactProxy>> queryContactsByCustomerIDAndStartAndLimit(string id, string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<ContactProxy>>() {
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
                return new Response<List<ContactProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID, s, l;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
                return new Response<List<ContactProxy>>() {
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

            return new Response<List<ContactProxy>>() {
				Code = 200,
				Message = "success.",
				Data = ContactProxy.FromContacts(contacts)
			};
		}

        public Response<CustomerProxy> queryCustomer(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<CustomerProxy>() {
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
                return new Response<CustomerProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

			int customerID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out customerID)) {
                return new Response<CustomerProxy>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Customer customer = entityContext.Customers
			                                 .Where(c => c.CustomerID == customerID)
			                                 .FirstOrDefault();
			//return response
            return new Response<CustomerProxy>() {
				Code = 200,
				Message = "success.",
				Data = new CustomerProxy(customer)
			};
		}

        public Response<List<CustomerProxy>> queryCustomersWithStartAndLimit(string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<CustomerProxy>>() {
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
                return new Response<List<CustomerProxy>>() {
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

            return new Response<List<CustomerProxy>>() {
				Code = 200,
				Message = "success.",
				Data = CustomerProxy.FromCustomers(customers)
			};
		}

        public Response<List<CustomerProxy>> queryCustomerWithQueryAndProperty(string query, string property) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<CustomerProxy>>() {
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
                return new Response<List<CustomerProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			if(string.IsNullOrEmpty(query) || string.IsNullOrEmpty(property)) {
                return new Response<List<CustomerProxy>>() {
					Code = 404,
					Message = "invalid query and property."
				};
			}

			switch(property) {
				case PROPERTY_NAME: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Firstname.Contains(query))
						                                        .ToList();
                        return new Response<List<CustomerProxy>>() {
							Code = 200,
							Message = "success.",
							Data = CustomerProxy.FromCustomers(customers)
						};
					}
				case PROPERTY_SURNAME: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Lastname.Contains(query))
																.ToList();
                        return new Response<List<CustomerProxy>>() {
							Code = 200,
							Message = "success.",
							Data = CustomerProxy.FromCustomers(customers)
						}; 
					}
				case PROPERTY_IDENTITY: {
						List<Customer> customers = entityContext.Customers
						                                        .Where(c => c.Identity.Contains(query))
																.ToList();
                        return new Response<List<CustomerProxy>>() {
							Code = 200,
							Message = "success.",
							Data = CustomerProxy.FromCustomers(customers)
						}; 
					}
				default:
                    return new Response<List<CustomerProxy>>() {
						Code = 404,
						Message = "unsupported property.",
						Data = null
					};
			}
		}

        public Response<List<CustomerProxy>> queryCustomers(string query) {
            string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
            if(string.IsNullOrEmpty(token)) {
                return new Response<List<CustomerProxy>>() {
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
                return new Response<List<CustomerProxy>>() {
                    Code = 401,
                    Message = "session expired.",
                    Data = null
                };
            }
            if(string.IsNullOrEmpty(query)) {
                return new Response<List<CustomerProxy>>() {
                    Code = 404,
                    Message = "invalid query and property."
                };
            }

            string[] queryArgs = Invoke<string[]>(() => {
                return query.Contains(',') ? query.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray() 
                                           : new string[] { query.Trim() };
            });

            if(queryArgs.Length == 1) {
                long identityNo;
                if(Int64.TryParse(queryArgs[0], out identityNo)) {
                    List<Customer> customers = entityContext.Customers.Where(x => x.Identity.Contains(identityNo.ToString())).ToList();
                    customers.ForEach(x => {
                        entityContext.Entry(x).Collection(y => y.Contacts).Load();
                        entityContext.Entry(x).Reference(y => y.Category).Load();
                        if(!x.Contacts.IsNullOrEmpty()) {
                            x.Contacts.ToList().ForEach(y => {
                                entityContext.Entry(y).Reference(p => p.Country).Load();
                                entityContext.Entry(y).Reference(p => p.City).Load();
                            });
                        }
                    });
                    return new Response<List<CustomerProxy>>() {
                        Code = 200,
                        Message = "success.",
                        Data = CustomerProxy.FromCustomers(customers)
                    };
                } else { 
                    string str = queryArgs[0];
                    //if string is category
                    Category category = entityContext.Categories.FirstOrDefault(x => x.CategoryName.Contains(str));
                    if(!category.IsNullOrEmpty()) {
                        List<Customer> customers = entityContext.Customers.Where(x => x.CategoryID == category.CategoryID).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });
                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    } else {
                        //check first name or middle name
                        List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(str)).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });

                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    }
                }
            } else if(queryArgs.Length == 2) {
                string firstName = queryArgs[0];
                long identityNo;
                if(Int64.TryParse(queryArgs[1], out identityNo)) {
                    List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                               || x.Identity.Contains(identityNo.ToString())).ToList();
                    customers.ForEach(x => {
                        entityContext.Entry(x).Collection(y => y.Contacts).Load();
                        entityContext.Entry(x).Reference(y => y.Category).Load();
                        if(!x.Contacts.IsNullOrEmpty()) {
                            x.Contacts.ToList().ForEach(y => {
                                entityContext.Entry(y).Reference(p => p.Country).Load();
                                entityContext.Entry(y).Reference(p => p.City).Load();
                            });
                        }
                    });
                    return new Response<List<CustomerProxy>>() {
                        Code = 200,
                        Message = "success.",
                        Data = CustomerProxy.FromCustomers(customers)
                    };
                } else {
                    string str = queryArgs[1];
                    //if string is category
                    Category category = entityContext.Categories.FirstOrDefault(x => x.CategoryName.Contains(str));
                    if(!category.IsNullOrEmpty()) {
                        List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                                   || x.CategoryID == category.CategoryID).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });
                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    } else {
                        List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                                   || x.Lastname.Contains(str)).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });
                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    }
                }
            } else if(queryArgs.Length == 3) {
                string firstName = queryArgs[0];
                string lastName = queryArgs[1];
                long identityNo;
                if(Int64.TryParse(queryArgs[2], out identityNo)) {
                    List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                               || x.Lastname.Contains(lastName)
                                                                               || x.Identity.Contains(identityNo.ToString())).ToList();
                    customers.ForEach(x => {
                        entityContext.Entry(x).Collection(y => y.Contacts).Load();
                        entityContext.Entry(x).Reference(y => y.Category).Load();
                        if(!x.Contacts.IsNullOrEmpty()) {
                            x.Contacts.ToList().ForEach(y => {
                                entityContext.Entry(y).Reference(p => p.Country).Load();
                                entityContext.Entry(y).Reference(p => p.City).Load();
                            });
                        }
                    });
                    return new Response<List<CustomerProxy>>() {
                        Code = 200,
                        Message = "success.",
                        Data = CustomerProxy.FromCustomers(customers)
                    };
                } else {
                    string str = queryArgs[2];
                    //if string is category
                    Category category = entityContext.Categories.FirstOrDefault(x => x.CategoryName.Contains(str));
                    if(!category.IsNullOrEmpty()) {
                        List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                                   || x.Lastname.Contains(lastName)
                                                                                   || x.CategoryID == category.CategoryID).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });
                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    } else {
                        //if it was str and there were nothing to compare so I made it as if they have such contact name
                        List<Customer> customers = entityContext.Customers.Where(x => x.Firstname.Contains(firstName)
                                                                                   || x.Lastname.Contains(lastName)
                                                                                   || x.Contacts.Where(y => y.ContactName.Contains(str)).Count() > 0).ToList();
                        customers.ForEach(x => {
                            entityContext.Entry(x).Collection(y => y.Contacts).Load();
                            entityContext.Entry(x).Reference(y => y.Category).Load();
                            if(!x.Contacts.IsNullOrEmpty()) {
                                x.Contacts.ToList().ForEach(y => {
                                    entityContext.Entry(y).Reference(p => p.Country).Load();
                                    entityContext.Entry(y).Reference(p => p.City).Load();
                                });
                            }
                        });
                        return new Response<List<CustomerProxy>>() {
                            Code = 200,
                            Message = "success.",
                            Data = CustomerProxy.FromCustomers(customers)
                        };
                    }
                }
            } else {
                return new Response<List<CustomerProxy>>() {
                    Code = 404,
                    Message = "too many arguments.",
                    Data = null
                };
            }
        }

        public Response<List<DirectoryProxy>> queryDirectoriesByCustomerID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
				return new Response<List<DirectoryProxy>>() {
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
				return new Response<List<DirectoryProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID;
			if(string.IsNullOrEmpty(id) || int.TryParse(id, out customerID)) {
				return new Response<List<DirectoryProxy>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Directory> directories = entityContext.Directories
			                                           .Where(d => d.CustomerID == customerID)
			                                           .ToList();
            directories.ForEach(x => {
                entityContext.Entry(x).Collection(p => p.Documents).Load();
            });
			return new Response<List<DirectoryProxy>>() {
				Code = 200,
				Message = "success.",
				Data = DirectoryProxy.FromDirectories(directories)
			};
		}

        public Response<List<DirectoryProxy>> queryDirectoriesByCustomerIDAndStartAndLimit(string id, string start, string limit) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<DirectoryProxy>>() {
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
                return new Response<List<DirectoryProxy>>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int customerID, s, l;
			if(string.IsNullOrEmpty(id) || int.TryParse(id, out customerID)) {
                return new Response<List<DirectoryProxy>>() {
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
            directories.ForEach(x => {
                entityContext.Entry(x).Collection(p => p.Documents).Load();
            });
            return new Response<List<DirectoryProxy>>() {
				Code = 200,
				Message = "success.",
                Data = DirectoryProxy.FromDirectories(directories)
			};
		}

        public Response<DirectoryProxy> queryDirectory(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<DirectoryProxy>() {
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
                return new Response<DirectoryProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int directoryID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out directoryID)) {
                return new Response<DirectoryProxy>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			Directory directory = entityContext.Directories
			                                   .Where(d => d.DirectoryID == directoryID)	
			                                   .FirstOrDefault();
            //load documents
            entityContext.Entry(directory).Collection(p => p.Documents).Load();
            return new Response<DirectoryProxy>() {
				Code = 200,
				Message = "success.",
				Data = new DirectoryProxy(directory)
			};
		}

        public Response<DocumentProxy> queryDocument(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<DocumentProxy>() {
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
                return new Response<DocumentProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			int documentID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out documentID)) {
                return new Response<DocumentProxy>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}

			Document document = entityContext.Documents
			                                 .Where(d => d.DocumentID == documentID)
			                                 .FirstOrDefault();

            return new Response<DocumentProxy>() {
				Code = 200,
				Message = "success.",
				Data = new DocumentProxy(document)
			};
		}

        // TODO: implement this
        public Response<List<DocumentProxy>> queryDocumentsByDirectoryAndStartAndLimit(string id, string start, string limit) {
            return new Response<List<DocumentProxy>>() {
                Code = 404,
                Message = "not implemented.",
                Data = null
            };
		}

        public Response<List<DocumentProxy>> queryDocumentsByDirectoryID(string id) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<List<DocumentProxy>>() { 
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
                return new Response<List<DocumentProxy>>() {
					Code = 401,
					Message = "unauthorized.",
					Data = null
				};
			}
			int directoryID;
			if(string.IsNullOrEmpty(id) || !int.TryParse(id, out directoryID)) {
                return new Response<List<DocumentProxy>>() {
					Code = 404,
					Message = "invalid id.",
					Data = null
				};
			}
			List<Document> documents = entityContext.Documents
			                                        .Where(d => d.DirectoryID == directoryID)
			                                        .ToList();
            return new Response<List<DocumentProxy>>() { 
				Code = 200,
				Message = "success.",
				Data = DocumentProxy.FromDocuments(documents)
			};
		}

        public Response<SessionProxy> signin(Sign sign) {
            try {
                if(string.IsNullOrEmpty(sign.UserName) || string.IsNullOrEmpty(sign.Password)) {
                    return new Response<SessionProxy>() {
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
                    return new Response<SessionProxy>() {
                        Code = 404,
                        Message = "username or password is invalid.",
                        Data = null
                    };
                }

                Session activeSession = entityContext.Sessions.Where(k => k.UserID == user.UserID
                                                                            && k.DueDate >= DateTime.Now)
                                                                .FirstOrDefault();

                if(activeSession != null) {
                    //extend it for 30 mins
                    activeSession.DueDate = DateTime.Now.AddMinutes(30);
                    entityContext.SaveChanges();

                    return new Response<SessionProxy>() {
                        Code = 200,
                        Message = "already logged in.",
                        Data = new SessionProxy() { Token = activeSession.Token }
                    };
                }
                //clean up old sessions
                entityContext.Sessions
                                .RemoveRange(entityContext.Sessions
                                                        .Where(m => m.DueDate <= DateTime.Now));

                Session session = new Session() {
                    UserID = user.UserID,
                    Token = Token.CreateToken(),
                    CreateDate = DateTime.Now,
                    DueDate = DateTime.Now.AddMinutes(30)
                };
                //store session
                entityContext.Sessions.Add(session);
                entityContext.SaveChanges();

                return new Response<SessionProxy>() {
                    Code = 200,
                    Message = "success.",
                    Data = new SessionProxy() { Token = session.Token }
                };
            } catch(Exception e) {
                return new Response<SessionProxy>() {
                    Code = 404,
                    Message = e.StackTrace,
                    Data = new SessionProxy() { 
                        Token = e.Message
                    }
                };
            }
		}

		public Response<bool> signout(string token) {
			if(string.IsNullOrEmpty(token)) {
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

        public Response<ContactProxy> updateContact(ContactProxy updateContact) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<ContactProxy>() {
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
                return new Response<ContactProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

            Contact contact = entityContext.Contacts
                                           .Where(c => c.ContactID == updateContact.CountactID)
                                           .FirstOrDefault();
            if(contact == null) {
                return new Response<ContactProxy>() {
                    Code = 404,
                    Message = "not exits, try create.",
                    Data = null
                };
            }

            contact.Address = updateContact.Address;
            contact.ContactName = updateContact.ContactName;
            contact.CityID = updateContact.CityID;
            contact.CountryID = updateContact.CountryID;
            contact.Phone = updateContact.Phone;
            contact.CustomerID = updateContact.CustomerID;

            entityContext.Entry(contact).State = EntityState.Modified;
            entityContext.SaveChanges();

            return new Response<ContactProxy>() {
                Code = 200,
                Message = "success.",
                Data = new ContactProxy(contact)
            };
		}

        public Response<CustomerProxy> updateCustomer(CustomerProxy updateCustomer) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<CustomerProxy>() {
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
                return new Response<CustomerProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}

            Customer customer = entityContext.Customers
                                             .Where(c => c.CustomerID == updateCustomer.CustomerID)
                                             .FirstOrDefault();

            if(customer == null) {
                return new Response<CustomerProxy>() { 
                    Code = 404,
                    Message = "not exists, try create.",
                    Data = null
                };
            }

            customer.CategoryID = updateCustomer.CategoryID;
            customer.Firstname = updateCustomer.FirstName;
            customer.Lastname = updateCustomer.LastName;
            customer.Identity = updateCustomer.Identity;
            customer.UpdateDate = DateTime.Now;

            entityContext.Entry(customer).State = EntityState.Modified;
            entityContext.SaveChanges();

            return new Response<CustomerProxy>() {
                Code = 200,
                Message = "success.",
                Data = new CustomerProxy(customer)
            };
		}

        public Response<DirectoryProxy> updateDirectory(DirectoryProxy updateDirectory) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<DirectoryProxy>() {
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
                return new Response<DirectoryProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists
            Directory directory = entityContext.Directories
                                               .Where(d => d.DirectoryID == updateDirectory.DirectoryID)
                                               .FirstOrDefault();

            if(directory == null) {
                return new Response<DirectoryProxy>() {
                    Code = 404,
                    Message  = "not exists, try create.",
                    Data = null
                };
            }

            directory.DirectoryID = updateDirectory.DirectoryID;
            directory.CustomerID = updateDirectory.CustomerID;
            directory.ParentDirectoryID = updateDirectory.ParentDirectoryID;
            directory.DirectoryName = updateDirectory.DirectoryName;
            directory.UserID = session.UserID;
            directory.UpdateDate = DateTime.Now;

            entityContext.Entry(directory).State = EntityState.Modified;
            entityContext.SaveChanges();

			return new Response<DirectoryProxy>() {
				Code = 200,
				Message = "success.",
				Data = new DirectoryProxy(directory)
			};		
		}

        public Response<DocumentProxy> updateDocument(DocumentProxy updateDocument) {
			string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
			if(string.IsNullOrEmpty(token)) {
                return new Response<DocumentProxy>() {
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
                return new Response<DocumentProxy>() {
					Code = 401,
					Message = "session expired.",
					Data = null
				};
			}
			//check if exists

            Document document = entityContext.Documents
                                             .Where(d => d.DocumentID == updateDocument.DocumentID)
                                             .FirstOrDefault();

            if(document == null) {
                return new Response<DocumentProxy>() {
                    Code = 404,
                    Message = "not exists, try create.",
                    Data = null
                };
            }

            document.DocumentName = updateDocument.DocumentName;
            document.CustomerID = updateDocument.CustomerID;
            document.ContentType = updateDocument.ContentType;
            document.ContentLength = updateDocument.ContentLength;
            document.DirectoryID = updateDocument.DirectoryID;
            document.UserID = session.UserID;
            document.UpdateDate = updateDocument.UpdateDate;//special case we use that update date as info bag

            entityContext.Entry(document).State = EntityState.Modified;
            entityContext.SaveChanges();

            return new Response<DocumentProxy>() {
                Code = 200,
                Message = "success.",
                Data = new DocumentProxy(document)
            };
        }

        public Response<List<CountryProxy>> queryCountries() {
            string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
            if(string.IsNullOrEmpty(token)) {
                return new Response<List<CountryProxy>>() {
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
                return new Response<List<CountryProxy>>() {
                    Code = 401,
                    Message = "session expired.",
                    Data = null
                };
            }

            return new Response<List<CountryProxy>>() {
                Code = 200,
                Message = "success.",
                Data = CountryProxy.FromCountries(entityContext.Countries.ToList())
            };
        }

        public Response<List<CityProxy>> queryCities(string id) {
            string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
            if(string.IsNullOrEmpty(token)) {
                return new Response<List<CityProxy>>() {
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
                return new Response<List<CityProxy>>() {
                    Code = 401,
                    Message = "session expired.",
                    Data = null
                };
            }

            int CountryID;
            bool isValid = Int32.TryParse(id, out CountryID);
            //CoutryID >= 1
            if(isValid && CountryID > 0) { 
                return new Response<List<CityProxy>>() {
                    Code = 200,
                    Message = "success.",
                    Data = CityProxy.FromCities(entityContext.Cities.Where(c => c.CountryID == CountryID).ToList())
                };
            }

            return new Response<List<CityProxy>>() {
                Code = 404,
                Message = "invlaid country.",
                Data = null
            };
        }

        public Response<List<CategoryProxy>> queryCategories() {
            string token = Token.GetToken(WebOperationContext.Current.IncomingRequest);
            if(string.IsNullOrEmpty(token)) {
                return new Response<List<CategoryProxy>>() {
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
                return new Response<List<CategoryProxy>>() {
                    Code = 401,
                    Message = "session expired.",
                    Data = null
                };
            }

            return new Response<List<CategoryProxy>>() {
                Code = 200,
                Message = "success.",
                Data = CategoryProxy.FromCategories(entityContext.Categories.ToList())
            };
        }

        public Response<int> version() {
            try {

                entityContext.Users.Select(x => x.CreateDate).ToArray();

                return new Response<int>() {
                    Code = 200,
                    Message = "success.",
                    Data = 1
                };
            } catch(Exception e) {
                return new Response<int>() {
                    Code = 404,
                    Message = e.StackTrace,
                    Data = -1
                };
            }
        }

        protected T Invoke<T>(Func<T> func) { return func(); } 
	}

	class Token {
		
		public static string CreateToken() {
            return Guid.NewGuid()
                       .ToString()
                       .Replace("-", "")
                       .ToUpper();
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

