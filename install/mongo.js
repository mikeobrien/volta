print("Creating volta db...");
volta = db.getSisterDB("volta")
volta.addUser("volta", "volta")

print("Creating voltatest db...");
voltatest = db.getSisterDB("voltatest")
voltatest.addUser("volta", "volta")