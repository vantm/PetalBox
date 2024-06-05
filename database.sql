-- Table: public.product

-- DROP TABLE IF EXISTS public.product;

CREATE TABLE IF NOT EXISTS public.product
(
    id character varying(36) COLLATE pg_catalog."default" NOT NULL,
    title character varying(200) COLLATE pg_catalog."default" NOT NULL,
    price numeric(9,2) NOT NULL,
    is_active boolean NOT NULL,
    quantity integer NOT NULL,
    CONSTRAINT "PK_product" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.product
    OWNER to postgres;

-- Table: public.transaction

-- DROP TABLE IF EXISTS public.transaction;

CREATE TABLE IF NOT EXISTS public.transaction
(
    id character varying(36) COLLATE pg_catalog."default" NOT NULL,
    product_id character varying(36) COLLATE pg_catalog."default" NOT NULL,
    title character varying(200) COLLATE pg_catalog."default" NOT NULL,
    quantity_changes integer NOT NULL,
    transaction_time timestamp with time zone NOT NULL,
    ref_id character varying(36) COLLATE pg_catalog."default",
    ref_type character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT tx_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.transaction
    OWNER to postgres;