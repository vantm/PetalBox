CREATE TABLE IF NOT EXISTS product
(
    id character varying(36) NOT NULL,
    title character varying(200) NOT NULL,
    price numeric(9,2) NOT NULL,
    is_active boolean NOT NULL,
    quantity integer NOT NULL,
    CONSTRAINT "PK_product" PRIMARY KEY (id)
)